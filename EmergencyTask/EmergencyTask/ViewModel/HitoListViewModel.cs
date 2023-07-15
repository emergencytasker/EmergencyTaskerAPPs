using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;

using EmergencyTask.API;
using EmergencyTask.API.ER;

using EmergencyTask.Model;

using EmergencyTask.ViewModel.Extensions;
using Rg.Plugins.Popup.Extensions;
using EmergencyTask.API.Enum;
using System.Threading.Tasks;
using System;
using EmergencyTask.Strings;
using Plugin.Net.Socket;
using Org.BouncyCastle.Asn1;

namespace EmergencyTask.ViewModel
{
    public class HitoListViewModel : ViewModelBase
    {

        #region BindableProperty Hito
        /// <summary>
        /// Hito de la propiedad bindable
        /// </summary>
        private HitoModel hito;
        public HitoModel Hito
        {
            get { return hito; }
            set { hito = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property AddHito
        /// <summary>
        /// AddHito
        /// </summary>
        private Command addhito;
        public Command AddHito
        {
            get { return addhito; }
            set { addhito = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Hitos
        /// <summary>
        /// Hitos de la propiedad bindable
        /// </summary>
        private ObservableCollection<HitoModel> hitos;
        public ObservableCollection<HitoModel> Hitos
        {
            get { return hitos; }
            set { hitos = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CanAddHito
        /// <summary>
        /// CanAddHito
        /// </summary>
        private bool canaddhito;
        public bool CanAddHito
        {
            get { return canaddhito; }
            set { canaddhito = value; OnPropertyChanged(); }
        }
        #endregion

        public int IdSolicitudServicio { get; set; }

        public HitoListViewModel(int idsolicitudservicio)
        {
            Hitos = new ObservableCollection<HitoModel>();
            IdSolicitudServicio = idsolicitudservicio;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;

            IsBusy = true;

            CanAddHito = usuario.idperfil == (int) Perfil.Client;
            AddHito = new Command(AddHito_Clicked);

            MessagingCenter.Instance.Subscribe<App, Hito>(App, "NewHito", NewHto);

            await Load();

            IsBusy = false;
        }

        private async Task Load()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            var source = await Client.Hito.Where(new Hito
            {
                cliente = usuario.Perfil == Perfil.Client ? usuario.id : 0,
                trabajador = usuario.Perfil == Perfil.Tasker ? usuario.id : 0,
                idsolicitudservicio = IdSolicitudServicio
            });

            IEnumerable<Ticket> tickets = new List<Ticket>(0);
            var ticketsrequest = await Client.Ticket.Where(s => s.idhito).In(source.Select(s => (object) s.id)).Execute();
            if (ticketsrequest.HasExecute)
            {
                tickets = ticketsrequest.Result;
            }
            
            RenderHitos(source, tickets);
        }

        private async void NewHto(App app, Hito hito)
        {
            IsBusy = true;
            if (hito != null)
                await NotifyHitosChanged(hito.idsolicitudservicio);
            await Load();
            IsBusy = false;
        }

        private async void AddHito_Clicked(object obj)
        {
            await Navigation.PushPopupAsync(new NuevoHitoPopup
            {
                BindingContext = new NuevoHitoViewModel(IdSolicitudServicio)
            });
        }

        private void RenderHitos(IEnumerable<Hito> source, IEnumerable<Ticket> tickets)
        {
            if (source == null) return;
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            var models = new List<HitoModel>(source.Count());
            foreach (var item in source)
            {
                var ticket = tickets.FirstOrDefault(t => t.idhito == item.id);
                var model = GetHito(item, ticket);
                if (model == null) continue;
                models.Add(model);
            }
            Hitos = new ObservableCollection<HitoModel>(models);
        }

        private HitoModel GetHito(Hito s, Ticket ticket)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return null;

            string state = "";

            var status = (HitoStatus) s.estado;
            switch (status)
            {
                case HitoStatus.AuthorizedFunds:
                    state = AppResource.Autorizado;
                    break;

                case HitoStatus.ReleaseFunds:
                    state = AppResource.Liberado;
                    break;

                case HitoStatus.RequestRefund:
                    state = AppResource.SolicitudReembolso;
                    break;

                case HitoStatus.Refund:
                    state = AppResource.Reembolsado;
                    break;
                case HitoStatus.Created:
                default:
                    state = AppResource.Creado;
                    break;
            }

            var final = me.Perfil == Perfil.Client ? s.cantidad : s.cantidad / 2;

            if(s.estado == (int) HitoStatus.ReleaseFunds) 
                final = me.Perfil == Perfil.Client ? s.costofinal : (s.cantidadtrabajador ?? 0);

            return new HitoModel
            {
                IdHito = s.id,
                Amount = $"${final}",
                Description = s.descripcion,
                State = state,
                IdState = (int)status,
                Price = s.costofinal > 0 ? s.costofinal : s.cantidad,
                TapMenu = new Command(TapMenu_clicked),
                Charge = s.chargeid,
                Cantidad = s.cantidad,
                TrabajoTerminado = s.trabajoterminado,
                Trabajador = s.trabajador,
                Cliente = s.cliente,
                TicketImage = Client.Path(ticket?.imagen ?? ""),
                TicketDetail = ticket?.detalle ?? "",
                HasTicket = ticket != null,
                TicketId = ticket?.id ?? -1,
                IsOptionsVisible = me.Perfil == Perfil.Client,
                RequestServiceId = s.idsolicitudservicio,
                IsFromExtras = s.extras == 1
            };
        }

        private async void TapMenu_clicked(object obj)
        {
            if (!(obj is HitoModel model)) return;
            if (!model.IsOptionsVisible) return;
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;

            if (usuario.Perfil == Perfil.Tasker)
            {
                await Transfer(model);
            }
            else
            {
                string autorizar = AppResource.Autorizar;
                string liberar = AppResource.Liberar;
                // string reembolsar = AppResource.Reembolzar;
                string verticket = AppResource.VerTicket;

                var options = GetOptions(model);
                if (options == null) return;

                var action = await ActionSheet(AppResource.Info, AppResource.Cancelar, options);
                if (action == autorizar)
                {
                    await RequestAutorization(model);
                }
                else if (action == liberar)
                {
                    await RequestRelease(model);
                }
                else /*if (action == reembolsar)
                {
                    await RequestRefund(model);
                }else */ if (action == verticket)
                {
                    await Navigation.PushPopupAsync(new TicketPopup
                    {
                        BindingContext = new TicketPopupViewModel(model)
                    });
                }
            }
        }

        private async Task Transfer(HitoModel model)
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            if (model.IdState != (int)HitoStatus.AuthorizedFunds || model.IdState != (int)HitoStatus.ReleaseFunds) return;
            if(!await Confirm(AppResource.TransferirABalance)) return;
            IsBusy = true;
            var appfee = model.TrabajoTerminado == 1;
            if (await AppExtension.TransferToBalance(model.IdHito, model.Cantidad, appfee))
            {
                Toast(AppResource.TransferenciaRealizada);
            }
            IsBusy = false;
        }

        private string[] GetOptions(HitoModel model)
        {
            string autorizar = AppResource.Autorizar;
            string liberar = AppResource.Liberar;
            // string reembolsar = AppResource.Reembolzar;
            string verticket = AppResource.VerTicket;
            var status = (HitoStatus) model.IdState;
            switch (status)
            {
                case HitoStatus.Created:
                    return new string[] { autorizar };
                case HitoStatus.AuthorizedFunds:
                    return new string[] { liberar };
                case HitoStatus.ReleaseFunds:
                    return model.HasTicket && model.TicketImage != Client.GetPath("") ? new string[] { verticket } : null;
            }
            return null;
        }

        private async Task RequestRelease(HitoModel model)
        {
            if (!await Confirm(AppResource.LiberarPago)) return;
            
            IsBusy = true;

            await Navigation.PushPopupAsync(new CodeValidationTaskPopup()
            {
                OK = new Action(async () =>
                {
                    IsBusy = true;
                    var usuario = Usuario.GetUserLogin();
                    if (usuario == null)
                    {
                        IsBusy = false;
                        Toast(AppResource.PagoNoLiberado);
                        return;
                    }
                    if(!await Release(model, usuario))
                    {
                        IsBusy = false;
                        Toast(AppResource.PagoNoLiberado);
                        return;
                    }
                    await NotifyHitosChanged(model.RequestServiceId);
                    IsBusy = false;
                })
            });

            IsBusy = false;
        }

        private async Task<bool> Release(HitoModel model, Usuario usuario)
        {
            var appfee = model.TrabajoTerminado == 1;
            if (!await usuario.ReleaseHito(model.Charge, model.Cantidad, model.IdHito, appfee)) return false;
            model.IdState = (int)HitoStatus.ReleaseFunds;
            model.State = AppResource.Liberado;
            return true;
        }

        private async Task RequestAutorization(HitoModel model)
        {
            if (!await Confirm(AppResource.AgregarSaldo)) return;

            IsBusy = true;

            await Navigation.PushPopupAsync(new CodeValidationTaskPopup()
            {
                OK = new Action(async () =>
                {
                    IsBusy = true;
                    var usuario = Usuario.GetUserLogin();
                    if (usuario == null)
                    {
                        IsBusy = false;
                        Toast(AppResource.PagoNoAutorizado);
                        return;
                    }
                    if (!await Autorization(model, usuario))
                    {
                        IsBusy = false;
                        Toast(AppResource.PagoNoAutorizado);
                        return;
                    }
                    await NotifyHitosChanged(model.RequestServiceId);
                    IsBusy = false;
                })
            });

            IsBusy = false;
        }

        private async Task NotifyHitosChanged(int idsolicitudservicio)
        {
            try
            {
                var socket = await SocketFactory.Instance.Resolve();
                await socket.Send($"Hitos-{idsolicitudservicio}", new Dictionary<string, string>
                {
                    { "Status", "Changed" }
                });
            }
            catch { }
        }

        private async Task<bool> Autorization(HitoModel model, Usuario usuario)
        {
            var chargeid = await usuario.AuthorizeHito(model.Price, model.Description, model.IdHito);
            if (string.IsNullOrEmpty(chargeid))
                return false;
            model.Charge = chargeid;
            model.IdState = (int)HitoStatus.AuthorizedFunds;
            model.State = AppResource.Autorizado;
            return true;
        }

        private async Task RequestRefund(HitoModel model)
        {
            if (!await Confirm(AppResource.SolicitudDeReembolso)) return;
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            IsBusy = true;
            if (!await usuario.RequestRefundHito(model.IdHito))
            {
                Toast(AppResource.ReembolsoNoAceptado);
                IsBusy = false;
                return;
            }
            var status = HitoStatus.RequestRefund;
            model.IdState = (int) status;
            model.State = AppResource.SolicitudReembolso;
            IsBusy = false;
        }
    }
}
