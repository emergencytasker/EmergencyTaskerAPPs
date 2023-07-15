using EmergencyTask.API;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class VerifyPhoneViewModel : ViewModelBase
    {
        #region BindableProperty Chronometer
        /// <summary>
        /// Chronometer de la propiedad bindable
        /// </summary>
        private string chronometer;
        public string Chronometer
        {
            get { return chronometer; }
            set { chronometer = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty A
        /// <summary>
        /// Á de la propiedad bindable
        /// </summary>
        private string a;
        public string A
        {
            get { return a; }
            set { a = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty B
        /// <summary>
        /// B de la propiedad bindable
        /// </summary>
        private string b;
        public string B
        {
            get { return b; }
            set { b = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty C
        /// <summary>
        /// C de la propiedad bindable
        /// </summary>
        private string c;
        public string C
        {
            get { return c; }
            set { c = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty D
        /// <summary>
        /// D de la propiedad bindable
        /// </summary>
        private string d;
        public string D
        {
            get { return d; }
            set { d = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty E
        /// <summary>
        /// E de la propiedad bindable
        /// </summary>
        private string e;
        public string E
        {
            get { return e; }
            set { e = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty F
        /// <summary>
        /// F de la propiedad bindable
        /// </summary>
        private string f;
        public string F
        {
            get { return f; }
            set { f = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnVerifyCode
        /// <summary>
        /// BtnVerifyCode de la propiedad bindable
        /// </summary>
        private UserCommand btnverifycode;
        public UserCommand BtnVerifyCode
        {
            get { return btnverifycode; }
            set { btnverifycode = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsVerifyCodeVisible
        /// <summary>
        /// IsVerifyCodeVisible de la propiedad bindable
        /// </summary>
        private bool isverifycodevisible;
        public bool IsVerifyCodeVisible
        {
            get { return isverifycodevisible; }
            set { isverifycodevisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsVerifyNumberVisible
        /// <summary>
        /// IsVerifyNumberVisible de la propiedad bindable
        /// </summary>
        private bool isverifynumbervisible;
        public bool IsVerifyNumberVisible
        {
            get { return isverifynumbervisible; }
            set { isverifynumbervisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnSendCode
        /// <summary>
        /// BtnSendCode de la propiedad bindable
        /// </summary>
        private UserCommand btnsendcode;
        public UserCommand BtnSendCode
        {
            get { return btnsendcode; }
            set { btnsendcode = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty PhoneUser
        /// <summary>
        /// PhoneUser de la propiedad bindable
        /// </summary>
        private string phoneuser;
        public string PhoneUser
        {
            get { return phoneuser; }
            set { phoneuser = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property EditNumber
        /// <summary>
        /// EditNumber
        /// </summary>
        private UserCommand editnumber;
        public UserCommand EditNumber
        {
            get { return editnumber; }
            set { editnumber = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Codes
        /// <summary>
        /// Codes
        /// </summary>
        private List<CodeModel> codes;
        public List<CodeModel> Codes
        {
            get { return codes; }
            set { codes = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Code
        /// <summary>
        /// Code
        /// </summary>
        private CodeModel code;
        public CodeModel Code
        {
            get { return code; }
            set { code = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnContinue
        /// <summary>
        /// BtnContinue
        /// </summary>
        private Command btncontinue;
        public Command BtnContinue
        {
            get { return btncontinue; }
            set { btncontinue = value; OnPropertyChanged(); }
        }
        #endregion

        public string CurrentCode { get; set; }

        public TimeSpan Time { get; set; }

        public string VerificationCode
        {
            get
            {
                return A + B + C + D + E + F;
            }
        }

        public int Intentos { get; set; } = 0;

        public VerifyPhoneViewModel()
        {
            
        }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            IsBusy = true;
            BtnSendCode = new UserCommand(BtnSendCode_Clicked);
            BtnVerifyCode = new UserCommand(BtnVerifyCode_Clicked);
            EditNumber = new UserCommand(EditNumber_Clicked);
            BtnContinue = new Command(Continue_Clicked);
            Codes = new List<CodeModel>
            {
                new CodeModel
                {
                    Code = "+52",
                    Country = "Mexico",
                    Text = "MEX +52"
                },
                new CodeModel
                {
                    Code = "+1",
                    Country = "EU",
                    Text = "EU +1"
                }
            };
            InitialState();
            IsBusy = false;
        }

        private void InitialState()
        {
            IsVerifyNumberVisible = true;
            IsVerifyCodeVisible = false;
        }

        private void SecondState()
        {
            IsVerifyNumberVisible = false;
            IsVerifyCodeVisible = true;
        }

        private void Continue_Clicked(object obj)
        {
            if (App.Perfil == API.Enum.Perfil.Client)
            {
                SetDetailPage(new HomePage());
            }
            else if(App.Perfil == API.Enum.Perfil.Tasker)
            {
                SetDetailPage(new WaitingServicePage
                {
                    BindingContext = new WaitingServiceViewModel()
                });
            }
        }

        private void EditNumber_Clicked(object obj, Usuario usuario)
        {
            IsVerifyNumberVisible = true;
            IsVerifyCodeVisible = false;
            PhoneUser = "";
        }

        private async void BtnVerifyCode_Clicked(object obj, Usuario usuario)
        {
            IsBusy = true;

            if (string.IsNullOrEmpty(VerificationCode))
            {
                Toast(AppResource.IngresarCodigoVerificacion);
                IsBusy = false;
                return;
            }

            if (CurrentCode != VerificationCode)
            {
                Toast(AppResource.CodigosNoCoinciden);
                IsBusy = false;
                return;
            }

            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                Toast(AppResource.NecesitasInternet);
                IsBusy = false;
                return;
            }

            var phone = Code.Code + PhoneUser;

            var update = await Client.User.Update(usuario.id, new Dictionary<string, string>
            {
                { nameof(Usuario.telefono), phone },
                { nameof(Usuario.telefonoverificado), "1" }
            });

            if(update != null && update.telefono == phone && update.telefonoverificado == 1)
            {

                usuario.telefonoverificado = 1;
                usuario.telefono = update.telefono;

                Usuario.SetUserLogin(usuario);

                if (App.Perfil == API.Enum.Perfil.Client)
                {
                    SetDetailPage(new HomePage());
                }
                else if (App.Perfil == API.Enum.Perfil.Tasker)
                {
                    SetDetailPage(new DocumentPage());
                }
            }
            else
            {
                Toast(AppResource.NoValidoCodigo);
            }

            IsBusy = false;
        }

        private void BtnSendCode_Clicked(object obj, Usuario usuario)
        {
            if (Code == null)
            {
                Toast(AppResource.CodigoPais);
                return;
            }
            IsBusy = true;
            var phone = Code.Code + PhoneUser;
            try
            {
                Valid(phone);
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
                IsBusy = false;
                return;
            }
            CurrentCode = new Random().Next(100000, 999999).ToString();
            if (Client.SendSms(phone, $"{usuario.nombre}, {string.Format(AppResource.UsaEsteCodigo, CurrentCode)}", ""))
            {
                SecondState();
                Time = new TimeSpan(0, 5, 0);
                OnSecondElapsed();
                Device.StartTimer(TimeSpan.FromSeconds(1), OnSecondElapsed);
            }
            else
            {
                Toast(AppResource.ErrorEnviarCodigo);
            }
            IsBusy = false;
        }

        private bool OnSecondElapsed()
        {
            Time = Time.Add(TimeSpan.FromSeconds(-1));
            var status = Time.TotalSeconds > 0;
            if (status)
            {
                Chronometer = $"{AppResource.ReenviarCodigo} {Time.ToString(@"mm\:ss")}";
            }
            else
            {
                Chronometer = AppResource.SolicitarNuevoCodigo;
                InitialState();
            }
            return status;
        }

        private void Valid(string phone)
        {
            Intentos++;
            if (Intentos == 3)
            {
                SaveNumberToBlackList(phone);
                Intentos = 0;
                throw new Exception(AppResource.CodigosMaximos);
            }
            if (IsNumberInBlackList(phone))
            {
                Intentos = 0;
                throw new Exception(AppResource.NumeroInvalido);
            }
        }

        private bool IsNumberInBlackList(string phone)
        {
            var variable = DataBase.GetVariable();
            return variable.LastNumbers != null && variable.LastNumbers.Contains(phone);
        }

        private void SaveNumberToBlackList(string phone)
        {
            var variable = DataBase.GetVariable();
            if (variable.LastNumbers == null) variable.LastNumbers = new List<string>();
            variable.LastNumbers.Add(phone);
            DataBase.SetVariable(variable);
        }
    }
}
