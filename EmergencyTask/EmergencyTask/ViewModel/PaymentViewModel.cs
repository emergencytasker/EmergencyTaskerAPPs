using Rg.Plugins.Popup.Extensions;
using Stripe;
using EmergencyTask.Model;
using System.Threading.Tasks;
using Xamarin.Forms;
using EmergencyTask.ViewModel.Extensions;
using EmergencyTask.ViewModel.Validators;
using EmergencyTask.API;
using EmergencyTask.Strings;
using Xamarin.Essentials;

namespace EmergencyTask.ViewModel
{
    public class PaymentViewModel : ViewModelBase
    {

        #region BindableProperty BtnAgregarVisible
        /// <summary>
        /// BtnAgregarVisible de la propiedad bindable
        /// </summary>
        private bool btnagregarvisible;
        public bool BtnAgregarVisible
        {
            get { return btnagregarvisible; }
            set { btnagregarvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnEditarVisible
        /// <summary>
        /// BtnEditarVisible de la propiedad bindable
        /// </summary>
        private bool btneditarvisible;
        public bool BtnEditarVisible
        {
            get { return btneditarvisible; }
            set { btneditarvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnEliminarVisible
        /// <summary>
        /// BtnEliminarVisible de la propiedad bindable
        /// </summary>
        private bool btneliminarvisible;
        public bool BtnEliminarVisible
        {
            get { return btneliminarvisible; }
            set { btneliminarvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty NumberCard
        /// <summary>
        /// NumberCard de la propiedad bindable
        /// </summary>
        private string numbercard;
        public string NumberCard
        {
            get { return numbercard; }
            set { numbercard = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TitularName
        /// <summary>
        /// TitularName de la propiedad bindable
        /// </summary>
        private string titularname;
        public string TitularName
        {
            get { return titularname; }
            set { titularname = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty ExpiryDate
        /// <summary>
        /// ExpiryDate de la propiedad bindable
        /// </summary>
        private string expirydate;
        public string ExpiryDate
        {
            get { return expirydate; }
            set { expirydate = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Brand
        /// <summary>
        /// Brand de la propiedad bindable
        /// </summary>
        private ImageSource brand;
        public ImageSource Brand
        {
            get { return brand; }
            set { brand = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnAgregar
        /// <summary>
        /// BtnAgregar de la propiedad bindable
        /// </summary>
        private Command btnagregar;
        public Command BtnAgregar
        {
            get { return btnagregar; }
            set { btnagregar = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnEditar
        /// <summary>
        /// BtnEditar de la propiedad bindable
        /// </summary>
        private Command btneditar;
        public Command BtnEditar
        {
            get { return btneditar; }
            set { btneditar = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnEliminar
        /// <summary>
        /// BtnEliminar de la propiedad bindable
        /// </summary>
        private Command btneliminar;
        public Command BtnEliminar
        {
            get { return btneliminar; }
            set { btneliminar = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CanAddCard
        /// <summary>
        /// CanAddCard
        /// </summary>
        private bool canaddcard;
        public bool CanAddCard
        {
            get { return canaddcard; }
            set { canaddcard = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Balance
        /// <summary>
        /// Balance
        /// </summary>
        private double balance;
        public double Balance
        {
            get { return balance; }
            set { balance = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoToInfo
        /// <summary>
        /// GoToInfo
        /// </summary>
        private Command gotoinfo;
        public Command GoToInfo
        {
            get { return gotoinfo; }
            set { gotoinfo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoToPayout
        /// <summary>
        /// GoToPayout
        /// </summary>
        private Command gotopayout;
        public Command GoToPayout
        {
            get { return gotopayout; }
            set { gotopayout = value; OnPropertyChanged(); }
        }
        #endregion

        public API.Stripe StripeClient { get; set; }
        public Card Card { get; set; }
        public string CustomerID { get; set; }

        public PaymentViewModel()
        {

        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            IsBusy = true;

            BtnAgregarVisible = false;
            BtnEditarVisible = false;
            BtnEliminarVisible = false;

            BtnAgregar = new Command(BtnAgregar_Clicked);
            BtnEditar = new Command(BtnEditar_Clicked);
            BtnEliminar = new Command(BtnEliminar_Clicked);
            GoToInfo = new Command(GoToInfo_Clicked);
            GoToPayout = new Command(GoToPayout_Clicked);

            MessagingCenter.Instance.Subscribe<PaymentViewModel, double>(this, "Payrequest", (viewmodel, retiro) =>
            {
                viewmodel.Balance -= retiro;
            }, this);

            await GetPayMethod();

            IsBusy = false;
        }

        private async void GoToPayout_Clicked(object obj)
        {
            IsBusy = true;
            var me = Usuario.GetUserLogin();
            if (me == null) return;

            var stripe = await App.GetStripeAsync();

            if (stripe == null)
            {
                Toast(AppResource.NoPodemosContinuar);
                return;
            }

            var customerid = await me.GetCustomerId();
            if (string.IsNullOrEmpty(customerid))
            {
                Toast(AppResource.NoPodemosContinuar);
                return;
            }

            if (!await me.HasPaymethod())
            {
                Toast(AppResource.DebesConfigurarCuentaParaRecibirFondos);
                return;
            }

            var card = await stripe.GetCustomerPaymethod(customerid);

            if (card == null)
            {
                Toast(AppResource.NoPodemosContinuar);
                return;
            }

            IsBusy = false;
            var accountnumber = await Promt<string>(AppResource.IngresaTuNumeroDeCuenta, AppResource.Cancelar, AppResource.Aceptar, Acr.UserDialogs.InputType.Default);
            if (string.IsNullOrEmpty(accountnumber))
            {
                Toast(AppResource.NumeroDeCuentaNoValido);
                return;
            }

            var routingnumber = await Promt<string>(AppResource.IngresaTuRoutingNumber, AppResource.Cancelar, AppResource.Aceptar, Acr.UserDialogs.InputType.Default);
            if (string.IsNullOrEmpty(routingnumber) || routingnumber.Length != 9)
            {
                Toast(AppResource.RoutingNumberNoValido);
                return;
            }

            double cantidad = await Promt<double>(string.Format(AppResource.IngresaLaCantidadARetirar, Balance), AppResource.Cancelar, AppResource.Aceptar, Acr.UserDialogs.InputType.Number);
            if (cantidad == default(double))
            {
                Toast(AppResource.CantidadNoValida);
                return;
            }
            else if (cantidad <= 0 || cantidad > Balance)
            {
                Toast(AppResource.CantidadNoValida);
                return;
            }

            IsBusy = true;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Toast(AppResource.SinInternet);
                IsBusy = false;
                return;
            }

            // registrar dato con saldo negativo en la tabla saldo
            // registrar en la tabla de pago la solicitud de pago
            var balance = await Client.Balance.Add(new API.ER.Balance
            {
                cantidad = cantidad * -1,
                descripcion = string.Format(AppResource.SolicitudDePagoA, accountnumber, routingnumber),
                essolicitud = 1,
                idusuario = me.id,
                idperfil = me.idperfil
            });

            if (balance == null || balance.id <= 0)
            {
                Toast(AppResource.NoPodemosContinuar);
                IsBusy = false;
                return;
            }

            var pago = await Client.Payout.Add(new API.ER.Payout
            {
                idsaldo = balance.id,
                routingnumber = routingnumber,
                accountnumber = accountnumber,
                cantidad = cantidad
            });

            if(pago == null || pago.id <= 0)
            {
                Toast(AppResource.NoPodemosContinuar);
                IsBusy = false;
                return;
            }

            Toast(AppResource.RetiroEnProceso);
            MessagingCenter.Instance.Send(this, "Payrequest", cantidad);

            GoToInfo?.Execute(null);
        }

        private async void GoToInfo_Clicked(object obj)
        {
            await Navigation.PushAsync(new PaymentHistoryPage());
        }

        private async Task GetPayMethod()
        {
            IsBusy = true;

            InternetValidator internet = new InternetValidator();
            if (!internet.Rule())
            {
                Toast(internet.ErrorMessage);
                return;
            }

            var usuario = Usuario.GetUserLogin();
            if (usuario == null)
            {
                Toast(AppResource.NoMostrarFormaPago);
                IsBusy = false;
                return;
            }

            StripeClient = await App.GetStripeAsync();

            if (StripeClient == null)
            {
                Toast(AppResource.NoMostrarFormaPago);
                IsBusy = false;
                return;
            }

            CustomerID = await usuario.GetCustomerId();
            if (!string.IsNullOrEmpty(CustomerID))
            {
                Card = await StripeClient.GetCustomerPaymethod(CustomerID);
                if(Card != null)
                {
                    SecureStorage.Remove("CardLast4");
                    await SecureStorage.SetAsync("CardLast4", Card.Last4);
                    NumberCard = $"**** **** **** {Card.Last4}";
                    TitularName = Card.Name;
                    ExpiryDate = $"{Card.ExpMonth}/{Card.ExpYear.ToString().Remove(0, 2)}";
                    Brand = $"{Card.Brand.Replace(" ", string.Empty).ToLower()}.png";
                    BtnAgregarVisible = false;
                    BtnEditarVisible = true;
                    BtnEliminarVisible = true;
                }
                else
                {
                    NoCard();
                }
            }
            else
            {
                NoCard();
            }

            await GetBalance();

            IsBusy = false;
        }

        private async Task GetBalance()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            Balance = await Client.GetBalance(usuario.id);
        }

        private void NoCard()
        {
            NumberCard = "---- ---- ---- ----";
            TitularName = AppResource.SinForma;
            ExpiryDate = "--/--";
            Brand = "";
            BtnAgregarVisible = true;
            BtnEditarVisible = false;
            BtnEliminarVisible = false;
        }

        private async void BtnEliminar_Clicked(object obj)
        {
            if (Card == null || string.IsNullOrEmpty(CustomerID)) return;
            if (await Confirm(AppResource.EliminarFormaPago))
            {
                if (await StripeClient.DeleteCustomerPaymethod(CustomerID, Card.Id))
                {
                    Toast($"{AppResource.SeEliminoFormaPago}");
                    NoCard();
                }
                else
                {
                    Toast(AppResource.NoEliminoFormaPago);
                }
            }
        }

        private async void BtnEditar_Clicked(object obj)
        {
            SuscribeToChanges();
            await Navigation.PushPopupAsync(new AddPaymentPopUp());
        }

        private async void BtnAgregar_Clicked(object obj)
        {
            SuscribeToChanges();
            await Navigation.PushPopupAsync(new AddPaymentPopUp());
        }

        private void SuscribeToChanges()
        {
            MessagingCenter.Unsubscribe<App>(App, "PaymethodAdded");
            MessagingCenter.Subscribe<App>(App, "PaymethodAdded", OnPaymenthodAdded);
        }

        private async void OnPaymenthodAdded(App app)
        {
            await GetPayMethod();
        }
    }
}
