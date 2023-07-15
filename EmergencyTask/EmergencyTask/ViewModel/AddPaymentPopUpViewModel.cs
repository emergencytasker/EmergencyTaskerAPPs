using EmergencyTask.API;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Extensions;
using EmergencyTask.ViewModel.Validators;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class AddPaymentPopUpViewModel : ViewModelBase
    {

        #region BindableProperty AddPayment
        /// <summary>
        /// AddPayment de la propiedad bindable
        /// </summary>
        private ExtendCommand addpayment;
        public ExtendCommand AddPayment
        {
            get { return addpayment; }
            set { addpayment = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Titular
        /// <summary>
        /// TitularName
        /// </summary>
        private string titular;
        public string Titular
        {
            get { return titular; }
            set { titular = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CardNumber
        /// <summary>
        /// CardNumber
        /// </summary>
        private string cardnumber;
        public string CardNumber
        {
            get { return cardnumber; }
            set { cardnumber = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property MMYY
        /// <summary>
        /// MMYY
        /// </summary>
        private string mmyy;
        public string MMYY
        {
            get { return mmyy; }
            set { mmyy = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CVC
        /// <summary>
        /// CVC
        /// </summary>
        private string cvc;
        public string CVC
        {
            get { return cvc; }
            set { cvc = value; OnPropertyChanged(); }
        }
        #endregion


        #region Notified Property BtnClose
        /// <summary>
        /// BtnClose
        /// </summary>
        private ICommand btnclose;
        public ICommand BtnClose
        {
            get { return btnclose; }
            set { btnclose = value; OnPropertyChanged(); }
        }
        #endregion
        public AddPaymentPopUpViewModel()
        {
            BtnClose = new Command(BtnClose_Clicked);
            AddPayment = new ExtendCommand(AddPayment_Clicked, new UserValidator(), new InternetValidator());
        }

        private async void AddPayment_Clicked(object arg1, IExecuteValidator[] validator)
        {
            if (!validator.TryGetComparator(out Usuario usuario)) return;
            IsBusy = true;

            var modo = await GetVar<string>("modo");

            if (string.IsNullOrEmpty(modo))
            {
                Toast(AppResource.SinFormaPago);
                return;
            }

            if (!FormIsValid()) 
            {
                IsBusy = false;
                return;
            }

            var data = MMYY.Split('/');
            long.TryParse(data[0], out long month);
            long.TryParse("20" + data[1], out long year);

            var client = await App.GetStripeAsync();
            
            if (client == null)
            {
                Toast(AppResource.SinFormaPago);
                IsBusy = false;
                return;
            }

            var token = await client.CreateToken(Titular, CardNumber, year, month, CVC);
            if (string.IsNullOrEmpty(token))
            {
                if (client.Error) Toast(client.ErrorMessage);
                else Toast(AppResource.ErrorTarjeta);
                IsBusy = false;
                return;
            }

            var stripeuserindb = (await Client.Stripeuser.Query(new API.ER.Stripeuser
            {
                idusuario = usuario.id,
                modo = modo
            }) ?? new List<API.ER.Stripeuser>()).FirstOrDefault();

            if(stripeuserindb != null && stripeuserindb.id > 0)
            {
                var customer = await client.UpdateCustomer(stripeuserindb.customer, usuario.email, token);
                if (string.IsNullOrEmpty(customer))
                {
                    if (client.Error) Toast(client.ErrorMessage);
                    else Toast(AppResource.NoActualizaFormaPago);
                }
                else
                {
                    Toast(AppResource.ActualizaFormaPago);
                    MessagingCenter.Send(App, "PaymethodAdded");
                    await Navigation.PopPopupAsync();
                }
            }
            else
            {
                var customer = await client.CreateCustomer(usuario.nombre, usuario.email, token, usuario.email, usuario.telefonoverificado == 1 ? usuario.telefono : "");
                if (string.IsNullOrEmpty(customer))
                {
                    if (client.Error) Toast(client.ErrorMessage);
                    else Toast(AppResource.NoAgregoFormaPago);
                    IsBusy = false;
                    return;
                }

                var stripeuser = await Client.Stripeuser.Add(new API.ER.Stripeuser
                {
                    modo = modo,
                    customer = customer,
                    idusuario = usuario.id
                });

                if(stripeuser != null && stripeuser.id > 0)
                {
                    Toast(AppResource.AgregoFormaPago);
                    MessagingCenter.Send(App, "PaymethodAdded");
                    if (PopupNavigation.Instance.PopupStack.Count > 0) await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    Toast(AppResource.ErrorFormaPago);
                }
            }

            IsBusy = false;
        }

        private async void BtnClose_Clicked(object obj)
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch { }
        }

        private bool FormIsValid()
        {
            if (string.IsNullOrEmpty(Titular))
            {
                Toast(AppResource.IngresaTitular);
                return false;
            }

            if (string.IsNullOrEmpty(CardNumber))
            {
                Toast(AppResource.IngresaNumeroTarjeta);
                return false;
            }

            if (string.IsNullOrEmpty(MMYY))
            {
                Toast(AppResource.IngresaFechaExpiracion);
                return false;
            }

            if (string.IsNullOrEmpty(CVC))
            {
                Toast(AppResource.IngresaCVC);
                return false;
            }

            return true;
        }
    }
}
