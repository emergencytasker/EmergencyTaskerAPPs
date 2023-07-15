using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.API.Response;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using Plugin.Social.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {

        #region BindableProperty Email
        /// <summary>
        /// Email de la propiedad bindable
        /// </summary>
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Password
        /// <summary>
        /// Password de la propiedad bindable
        /// </summary>
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty SignIn
        /// <summary>
        /// SignIn de la propiedad bindable
        /// </summary>
        private Command signin;
        public Command SignIn
        {
            get { return signin; }
            set { signin = value; OnPropertyChanged(); }
        }
        #endregion
            
        #region BindableProperty TapPassword
        /// <summary>
        /// TapPassword de la propiedad bindable
        /// </summary>
        private Command tappassword;
        public Command TapPassword
        {
            get { return tappassword; }
            set { tappassword = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty SignUp
        /// <summary>
        /// SignUp de la propiedad bindable
        /// </summary>
        private Command signup;
        public Command SignUp
        {
            get { return signup; }
            set { signup = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Facebook
        /// <summary>
        /// Facebook de la propiedad bindable
        /// </summary>
        private Command facebook;
        public Command Facebook
        {
            get { return facebook; }
            set { facebook = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapContinue
        /// <summary>
        /// TapContinue de la propiedad bindable
        /// </summary>
        private Command tapcontinue;
        public Command TapContinue
        {
            get { return tapcontinue; }
            set { tapcontinue = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsClient
        /// <summary>
        /// IsClient
        /// </summary>
        private bool isclient;
        public bool IsClient
        {
            get { return isclient; }
            set { isclient = value; OnPropertyChanged(); }
        }
        #endregion

        public LoginViewModel()
        {
            SignUp = new Command(SignUp_Clicked);
            SignIn = new Command(SignIn_Clicked);
            TapPassword = new Command(TapPassword_Clicked);
            TapContinue = new Command(TapContinue_Clicked);
            Facebook = new Command(Facebook_Clicked);

#if DEBUG
            if(App.Perfil == Perfil.Client)
            {
                Email = "adriana_cano@devazt.com";
                Password = "123456";
            }
            else if(App.Perfil == Perfil.Tasker)
            {
                Email = "nekszer@gmail.com";
                Password = "010409";
            }
#endif
            IsClient = App.Perfil == Perfil.Client;
        }

        private async void Facebook_Clicked(object obj)
        {
            var facebookid = await GetVar<string>("facebookid");
            
            if(string.IsNullOrEmpty(facebookid))
            {
                Toast(AppResource.InicioSesionFallido);
                return;
            }

            FacebookOAuthRequest request = new FacebookOAuthRequest(facebookid, scope: "email");
            request.AccessTokenResult += Request_AccessTokenResult;
            await Navigation.PushAsync(request);
        }

        private async void Request_AccessTokenResult(object sender, FacebookOAuthResult e)
        {
            await Navigation.PopAsync();
            IsBusy = true;

            if(Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                Toast(AppResource.SinInternet);
                IsBusy = false;
                return;
            }

            var api = e.Api;
            var me = await api.Explorer<FbUser>("/v3.2/me?fields=id,first_name,last_name,email");
            if (me == null || string.IsNullOrEmpty(me.id))
            {
                IsBusy = false;
                return;
            }
            var path = await GetFbProfileImage(api, me);

            var indb = ((await Client.User.Where(new User
            {
                email = me.email
            })) ?? new List<User>()).FirstOrDefault();

            if (indb == null)
            {
                var user = await Client.User.Add(new User
                {
                    email = me.email,
                    nombre = me.first_name + " " + me.last_name,
                    password = me.id,
                    idperfil = (int)App.Perfil,
                    facebooklogin = 1,
                    facebookid = me.id,
                    activado = 1,
                    imagen = path,
                    lenguaje = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName
                });

                if (user == null || user.id <= 0)
                {
                    Toast(AppResource.FacebookFallido);
                    return;
                }
            }

            await Authentication(me.email, me.id);

            IsBusy = false;
        }

        /// <summary>
        /// Trata de obtener una imagen del perfil del usuario
        /// </summary>
        /// <param name="api"></param>
        /// <param name="me"></param>
        /// <returns></returns>
        private async Task<string> GetFbProfileImage(GraphApi api, FbUser me)
        {
            var fbimage = await api.Explorer<FbImage>($"/v3.2/{me.id}/picture?height=320&width=320");

            if (fbimage != null && fbimage.data != null)
            {
                var imageurl = fbimage.data.url;
                var bytes = Client.DownloadData(imageurl);
                if (bytes == null)
                {
                    return string.Empty;
                }
                var upload = await Client.Upload(bytes);
                if(upload == null)
                {
                    return string.Empty;
                }
                return upload.path;
            }

            return string.Empty;
        }

        private async void TapContinue_Clicked(object obj)
        {
            await Navigation.PushAsync(new HomePage());
        }

        private async void TapPassword_Clicked(object obj)
        {
            await Navigation.PushAsync(new PasswordPage());
        }

        private async void SignIn_Clicked(object obj)
        {
            if (string.IsNullOrEmpty(Email))
            {
                Toast(AppResource.IngresaEmail);
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                Toast(AppResource.IngresaPassword);
                return;
            }

            Email = Email.TrimEnd().TrimStart().Trim();
            Password = Password.TrimEnd().TrimStart().Trim();

            try
            {
                new MailAddress(Email);
            }
            catch
            {
                Toast(AppResource.IngresaUnEmailValido);
                return;
            }

            await Authentication(Email, Password);
        }

        public async Task Authentication(string email, string password)
        {
            IsBusy = true;
            var auth = await Client.Auth(email, password, Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            if (!auth.status)
            {
                await ProcessError(auth);
                IsBusy = false;
                return;
            }

            Client.SetToken(auth.token);
            var user = await Client.User.Get(auth.code);
            if (user == null)
            {
                Toast(AppResource.ErrorInicioSesion);
                IsBusy = false;
                return;
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            var usuario = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuario>(json);
            usuario.token = auth.token;
            usuario.password = Password;
            Usuario.SetUserLogin(usuario);
            if (usuario.Perfil != App.Perfil)
            {
                var currentapp = App.Perfil.ToString();
                var yourprofile = usuario.Perfil.ToString();
                Toast(string.Format(AppResource.IniciaSesionApp, yourprofile, currentapp));
                IsBusy = false;
                return;
            }

            await SecureStorage.SetAsync("sessionid", auth.sessionid.ToString());

            if (await HasPendingService())
            {
                await Navigation.PopAsync();
                IsBusy = false;
                return;
            }

            if (usuario.Perfil == Perfil.Client)
            {
                if (string.IsNullOrEmpty(usuario.telefono) || usuario.telefonoverificado == 0)
                {
                    SetDetailPage(new VerifyPhonePage());
                }
                else
                {
                    await OnBasedProfileOpenApp(false);
                }
            }
            else if (usuario.Perfil == Perfil.Tasker)
            {
                if (string.IsNullOrEmpty(usuario.telefono) || usuario.telefonoverificado == 0)
                {
                    SetDetailPage(new VerifyPhonePage());
                }
                else if (string.IsNullOrEmpty(usuario.identificacion) || string.IsNullOrEmpty(usuario.segurosocial))
                {
                    SetDetailPage(new DocumentPage());
                }
                else
                {
                    await OnBasedProfileOpenApp(false);
                }
            }

            IsBusy = false;
        }

        /// <summary>
        /// Permite saber si existe un servicio pendiente en formulario
        /// </summary>
        /// <returns></returns>
        private async Task<bool> HasPendingService()
        {
            var pendingservice = await SecureStorage.GetAsync("pendingservice");
            ServiceModel model = null;
            try
            {
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceModel>(pendingservice);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return model != null;
        }

        private async Task ProcessError(Auth auth)
        {
            if (auth.code == (int)AuthCode.MailNotVerified)
            {
                Toast(AppResource.ConfirmarCorreo);
            }

            if (auth.code == (int)AuthCode.PasswordNotMatch)
            {
                await RecoveryPassword(auth.message);
            }
            else
            {
                Toast(auth.message);
            }
        }

        private async Task RecoveryPassword(string message)
        {
            if (!await Confirm(message + "," + AppResource.RecuperarContraseña)) return;
            await Navigation.PushAsync(new PasswordPage());
        }

        private async void SignUp_Clicked(object obj)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}