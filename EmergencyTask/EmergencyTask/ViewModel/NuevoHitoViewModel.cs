using System;
using System.Threading.Tasks;
using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Extensions;
using EmergencyTask.ViewModel.Validators;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class NuevoHitoViewModel : ViewModelBase
    {

        #region BindableProperty Cantidad
        /// <summary>
        /// Cantidad de la propiedad bindable
        /// </summary>
        private double cantidad;
        public double Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Descripcion
        /// <summary>
        /// Descripcion de la propiedad bindable
        /// </summary>
        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Preautorizado
        /// <summary>
        /// Preautorizado de la propiedad bindable
        /// </summary>
        private bool preautorizado;
        public bool Preautorizado
        {
            get { return preautorizado; }
            set { preautorizado = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Liberado
        /// <summary>
        /// Liberado de la propiedad bindable
        /// </summary>
        private bool liberado;
        public bool Liberado
        {
            get { return liberado; }
            set { liberado = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnGuardar
        /// <summary>
        /// BtnGuardar de la propiedad bindable
        /// </summary>
        private ExtendCommand btnguardar;
        public ExtendCommand BtnGuardar
        {
            get { return btnguardar; }
            set { btnguardar = value; OnPropertyChanged(); }
        }
        #endregion

        public int IdSolicitudServicio { get; set; }
        public string CustomerId { get; set; }
        public int IdTrabajador { get; set; }
        public string Email { get; set; }
        public API.Stripe Stripe { get; set; }

        public NuevoHitoViewModel(int idsolicitudservicio)
        {
            IdSolicitudServicio = idsolicitudservicio;
        }

        /// <summary>
        /// Se realiza cuando la ventana aparece
        /// </summary>
        /// <param name="page"></param>
        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            if (!await EnsureApis())
            {
                await TryGoBack();
                return;
            }

            Preautorizado = false;
            BtnGuardar = new ExtendCommand(BtnGuardar_Clicked, new UserValidator(), new InternetValidator());
        }

        /// <summary>
        /// Intenta navegar hacia atras
        /// </summary>
        /// <returns></returns>
        private async Task TryGoBack()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await Navigation.PopPopupAsync();
            }
        }

        /// <summary>
        /// Verifica que todas las APIs esten correctamente antes de la ejecucion de la vista
        /// </summary>
        /// <returns></returns>
        private async Task<bool> EnsureApis()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null)
            {
                Toast(AppResource.SinSesionIniciada);
                return false;
            }

            Email = usuario.email;

            Stripe = await App.GetStripeAsync();
            if (Stripe == null)
            {
                Toast(AppResource.SinVista);
                return false;
            }

            if (!await usuario.HasPaymethod())
            {
                Toast(AppResource.RequieresPago);
                return false;
            }

            CustomerId = await usuario.GetCustomerId();

            if (string.IsNullOrEmpty(CustomerId))
            {
                Toast(AppResource.NoSePuedeAcceder);
                return false;
            }

            var solicitudservicio = await Client.Requestservice.Get(IdSolicitudServicio);
            if (solicitudservicio == null)
            {
                Toast(AppResource.NoPodemosAcceder);
                return false;
            }

            IdTrabajador = solicitudservicio.trabajador;

            if (IdTrabajador <= 0) return false;

            return true;
        }

        /// <summary>
        /// Boton guardar
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void BtnGuardar_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (!FormIsValid())
            {
                IsBusy = false;
                return;
            }

            if(!await Confirm($"{AppResource.CantidadDe} {Cantidad}?"))
            {
                IsBusy = false;
                return;
            }

            IsBusy = true;

            Hito operation = null;

            if (Preautorizado)
            {
                if (Liberado)
                {
                    operation = await CreatePayment(true);
                }
                else
                {
                    operation = await CreatePayment(false);
                }
            }
            else
            {
                operation = await CreateHito();
            }

            if (operation == null)
            {
                Toast(AppResource.NoRegistroPago);
                IsBusy = false;
                return;
            }

            Toast(AppResource.RegistroPago);

            MessagingCenter.Instance.Send(App, "NewHito", operation);

            await TryGoBack();

            IsBusy = false;
        }

        /// <summary>
        /// Crea un hito normal
        /// </summary>
        /// <returns></returns>
        private async Task<Hito> CreateHito()
        {
            var hito = GetHito();
            if (hito == null) return null;
            return await InsertHito(hito);
        }

        /// <summary>
        /// Devuelve la configuracion de un hito segun la configuracion del formulario
        /// </summary>
        /// <param name="charge"></param>
        /// <param name="fechaautorizacion"></param>
        /// <param name="fechaliberacion"></param>
        /// <returns></returns>
        private Hito GetHito(string charge = "", string fechaautorizacion = "", string fechaliberacion = "")
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return null;

            var status = (int) HitoStatus.Created;

            if (!Preautorizado)
            {
                status = (int)HitoStatus.Created;
            }
            else
            {
                if (Liberado)
                {
                    status = (int)HitoStatus.ReleaseFunds;
                }
                else
                {
                    status = (int)HitoStatus.AuthorizedFunds;
                }
            }

            return new Hito
            {
                estado = status,
                cantidad = Cantidad,
                descripcion = descripcion,
                cliente = usuario.id,
                trabajador = IdTrabajador,
                idsolicitudservicio = IdSolicitudServicio,
                chargeid = charge,
                fechadeautorizacion = fechaautorizacion,
                fechadeliberacion = fechaliberacion
            };
        }

        /// <summary>
        /// Inserta un hito a la base de datos
        /// </summary>
        /// <param name="hito"></param>
        /// <returns></returns>
        private async Task<Hito> InsertHito(Hito hito)
        {
            var hitoinweb = await Client.Hito.Add(hito);
            if (hitoinweb == null || hitoinweb.id <= 0) return null;
            return hitoinweb;
        }

        /// <summary>
        /// Crea un pago en stripe
        /// </summary>
        /// <param name="capture">Permite definir si se captura o no el pago</param>
        /// <returns></returns>
        private async Task<Hito> CreatePayment(bool capture)
        {
            var date = await Client.GetDate();
            if (date == null)
            {
                Toast(AppResource.SinConfigurarHora);
                return null;
            }
            var now = date.Value;
            if (!FormIsValid()) return null;
            var costincentavos = Cantidad * 100D;
            long.TryParse(costincentavos.ToString(), out long stripecost);
            if (stripecost == 0)
            {
                Toast(AppResource.NoAgendarServicio);
                IsBusy = false;
                return null;
            }
            var charge = await Stripe.CreateCharge((long)Cantidad * 100, "usd", CustomerId, Descripcion, Email, capture);
            if (string.IsNullOrEmpty(charge))
            {
                Toast(AppResource.SinAgendarServicio);
                IsBusy = false;
                return null;
            }
            var fecha = now.ToMySqlDateTimeFormat();
            var hito = GetHito(charge, fecha, fecha);
            if (hito == null) return null;
            return await InsertHito(hito);
        }

        /// <summary>
        /// Verifica que el formulario sea correcto
        /// </summary>
        /// <returns></returns>
        private bool FormIsValid()
        {
            if(Cantidad <= 0) return false;
            if(string.IsNullOrEmpty(Descripcion)) return false;
            if (Preautorizado)
            {
                if (Stripe == null) return false;
                if (string.IsNullOrEmpty(CustomerId)) return false;
            }
            return true;
        }
    }
}
