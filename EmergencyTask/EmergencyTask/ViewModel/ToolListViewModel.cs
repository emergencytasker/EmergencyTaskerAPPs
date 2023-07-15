using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class ToolListViewModel : ViewModelBase
    {

        #region BindableProperty Tool
        /// <summary>
        /// Tool de la propiedad bindable
        /// </summary>
        private ToolModel tool;
        public ToolModel Tool
        {
            get { return tool; }
            set { tool = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Tools
        /// <summary>
        /// Tools de la propiedad bindable
        /// </summary>
        private ObservableCollection<ToolModel> tools;
        public ObservableCollection<ToolModel> Tools
        {
            get { return tools; }
            set { tools = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property AddCommand
        /// <summary>
        /// AddCommand
        /// </summary>
        private Command addcommand;
        public Command AddCommand
        {
            get { return addcommand; }
            set { addcommand = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnContinuar
        /// <summary>
        /// BtnContinuar
        /// </summary>
        private Command btncontinuar;
        public Command BtnContinuar
        {
            get { return btncontinuar; }
            set { btncontinuar = value; OnPropertyChanged(); }
        }
        #endregion

        public int IdSolicitudServicio { get; set; }
        public Requestservice CurrentService { get; set; }
        public Ticket Ticket { get; set; }
        public Hito Hito { get; set; }

        public ToolListViewModel(Requestservice requestservice)
        {
            CurrentService = requestservice ?? throw new NullReferenceException(AppResource.SinServicio);
            IdSolicitudServicio = CurrentService.id;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;

            IsBusy = true;

            if(Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                Toast(AppResource.SinInternet);
                IsBusy = false;
                return;
            }

            if (!await EnsureApis())
            {
                IsBusy = false;
                return;
            }

            await LoadAccesories();

            AddCommand = new Command(AddCommand_Clicked);
            BtnContinuar = new Command(BtnContinuar_Clicked);
            IsBusy = false;
        }

        private async Task<bool> EnsureApis()
        {
            Ticket = (await Client.Ticket.Where(new Ticket
            {
                idsolicitudservicio = IdSolicitudServicio
            })).LastOrDefault(t => t.idhito.HasValue);

            if (Ticket == null || Ticket.id <= 0) return true;

            Hito = (await Client.Hito.Where(new Hito
            {
                id = Ticket.idhito.Value
            })).LastOrDefault();

            if (Hito != null && Hito.estado == (int)HitoStatus.AuthorizedFunds)
            {
                Toast(AppResource.HerramientasMaximo);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Carga la lista de accesorios del servicio
        /// </summary>
        /// <returns></returns>
        private async Task LoadAccesories()
        {
            var accesorios = (await Client.Accessory.Query(new Accessory
            {
                idsolicitudservicio = IdSolicitudServicio
            }) ?? new List<Accessory>());

            Tools = new ObservableCollection<ToolModel>(accesorios.Select(a => GetModel(a)));
        }

        private ToolModel GetModel(Accessory a)
        {
            if (a == null) return null;
            return new ToolModel
            {
                Id = a.id,
                Nombre = a.nombre,
                Cantidad = a.cantidad,
                Costo = (a.costo ?? 0).ToString(),
                IsActionVisible = true,
                IsSubtitleEntryVisible = true,
                IsSubtitleLabelVisible = false,
                Action = new ExtendCommand(BtnSave_Clicked, new UserValidator(), new InternetValidator()),
                Command = new Command(TapLabel_Clicked),
                Update = new Command(Update_Clicked),
                Delete = new Command(Delete_Clicked)
            };
        }

        private async void AddCommand_Clicked(object obj)
        {
            await Navigation.PushPopupAsync(new AgregarAccesorioPopup(async (model) =>
            {
                IsBusy = true;
                await Navigation.PopPopupAsync();
                var accesorio = await Client.Accessory.Add(new Accessory
                {
                    cantidad = model.Cantidad,
                    nombre = model.Nombre,
                    idsolicitudservicio = IdSolicitudServicio
                });
                if (accesorio != null)
                {
                    Tools.Add(GetModel(accesorio));
                }
                else
                {
                    Toast(AppResource.NoGuardoAccesorio);
                }
                IsBusy = false;
            }));
        }

        private async void Delete_Clicked(object obj)
        {
            if (!(obj is ToolModel model)) return;
            if(await Confirm(AppResource.EliminarAccesorio))
            {
                IsBusy = true;
                var delete = await Client.Accessory.Delete(model.Id);
                if(delete.HasBeenDeleted(model.Id))
                {
                    Tools.Remove(model);
                }
                IsBusy = false;
            }
        }

        private async void Update_Clicked(object obj)
        {
            if (!(obj is ToolModel model)) return;
            await Navigation.PushPopupAsync(new AgregarAccesorioPopup(async (newmodel) =>
            {
                IsBusy = true;
                await Navigation.PopPopupAsync();

                var update = await Client.Accessory.Update(model.Id, new Dictionary<string, string>
                {
                    { nameof(Accessory.cantidad), newmodel.Cantidad.ToString() },
                    { nameof(Accessory.nombre), newmodel.Nombre }
                });

                if(update != null && update.cantidad == newmodel.Cantidad && update.nombre == newmodel.Nombre)
                {
                    model.Nombre = update.nombre;
                    model.Cantidad = update.cantidad;
                    model.Costo = (update.costo ?? 0).ToString();
                }

                IsBusy = false;
            }, new AccesorioModel
            {
                Nombre = model.Nombre,
                Cantidad = model.Cantidad
            }));
        }

        private async void BtnContinuar_Clicked(object obj)
        {
            IsBusy = true;
            var total = 0D;
            try
            {
                total = Tools.Sum(t => double.Parse(t.Costo) * t.Cantidad);
            }
            catch { }

            if (total <= 0)
            {
                Toast(AppResource.PresupuestoInvalido);
                IsBusy = false;
                return;
            }

            if (await Confirm(AppResource.EnviarPresupuesto))
            {
                if (Hito == null)
                {
                    Hito = await Client.Hito.Add(new Hito
                    {
                        cantidad = total,
                        idsolicitudservicio = IdSolicitudServicio,
                        cliente = CurrentService.cliente,
                        trabajador = CurrentService.trabajador,
                        descripcion = AppResource.ListaMateriales
                    });
                }
                else
                {
                    Hito = await Client.Hito.Update(Hito.id, new Dictionary<string, string>
                    {
                        { nameof(Hito.cantidad), total.ToString() }
                    });
                }

                if(Hito == null)
                {
                    Toast(AppResource.NoGuardoAccesorios);
                    IsBusy = false;
                    return;
                }

                if (Ticket == null)
                {
                    Ticket = await Client.Ticket.Add(new Ticket
                    {
                        idsolicitudservicio = IdSolicitudServicio,
                        total = total,
                        idhito = Hito.id
                    });
                }
                else
                {
                    Ticket = await Client.Ticket.Update(Ticket.id, new Dictionary<string, string>
                    {
                        { nameof(Ticket.total), total.ToString() }
                    });
                }

                if (Ticket == null) 
                {
                    Toast(AppResource.NoGuardoAccesorios);
                    IsBusy = false;
                    return;
                }

                List<Accessory> accesories = new List<Accessory>();

                foreach (var tool in Tools)
                {
                    var accesory = await Client.Accessory.Update(tool.Id, new Dictionary<string, string>
                    {
                        { nameof(Accessory.idticket), Ticket.id.ToString() }
                    });

                    if (accesory == null) continue;

                    accesories.Add(accesory);
                }

                if(accesories.Count(a => a.idticket == Ticket.id) != Tools.Count)
                {
                    Toast(AppResource.NoGuardoAccesorios);
                    IsBusy = false;
                    return;
                }

                Toast(AppResource.PresupuestoEnviado);
                try { await Navigation.PopAsync(); } catch { }
                IsBusy = false;
            }
        }

        private void TapLabel_Clicked(object obj)
        {
            if (!(obj is ToolModel model)) return;
            model.IsActionVisible = true;
            model.IsSubtitleEntryVisible = true;
            model.IsSubtitleLabelVisible = false;
        }

        private async void BtnSave_Clicked(object obj, IExecuteValidator[] validators)
        {
            if (!(obj is ToolModel model)) return;
            IsBusy = true;

            var accessory = await Client.Accessory.Update(model.Id, new Dictionary<string, string>
            {
                { nameof(Accessory.costo), model.Costo }
            });

            if(accessory != null && accessory.costo.ToString() == model.Costo)
            {
                model.Costo = accessory.costo.ToString();
                model.IsActionVisible = false;
                model.IsSubtitleEntryVisible = false;
                model.IsSubtitleLabelVisible = true;
            }

            IsBusy = false;
        }
    }
}