using EmergencyTask.API.Enum;
using EmergencyTask.Helpers;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification;
using Plugin.Language;
using Plugin.Net.Socket;
using Plugin.Notification;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask
{
    public partial class App : Application
    {

        /// <summary>
        //Perfil actual de la app [contratista, trabajador]
        /// </summary> 
        public Perfil Perfil { get; set; }
        public static DisplayInfoHelper Display { get; set; }

        /// <summary>
        /// Parametros de una notificacion
        /// </summary>
        public Dictionary<string,string> Parameters { get; set; }

        public App(Perfil app, Dictionary<string, string> parameters = null)
        {
            Perfil = app;
            Parameters = parameters;
            Display = new DisplayInfoHelper();
            InitializeComponent();
            TranslateExtension.Init(AppResource.ResourceManager);
            MainPage = new NavigationPage(new MainPage());
        }

        protected override async void OnStart()
        {
            NotificationHelper.Start();
            NotificationHelper.Init();
            Parameters = await LocalNotification.OnOpen(Parameters);
            if (Parameters == null) await ViewModelBase.OnBasedProfileOpenApp();
            if (Perfil == Perfil.Client)
            {
                AppCenter.Start("android=5f512e05-70b4-478e-bfcc-a773251af974;" +
                      "uwp={Your UWP App secret here};" +
                      "ios=0f48be9f-6112-4587-b3f7-8018f8374a2c", typeof(Analytics), typeof(Crashes));
            }
            else if(Perfil == Perfil.Tasker)
            {
                AppCenter.Start("android=e724460a-50c7-4a49-9964-81e8241cceb5;" +
                      "uwp={Your UWP App secret here};" +
                      "ios=af5a216e-2277-49aa-b48d-0816f4555e4e", typeof(Analytics), typeof(Crashes));
            }

            
            SocketFactory.Instance.Register(async () =>
            {
                await Task.Delay(1);
                return new SocketIO("http://142.11.222.110:9182/");
            });

            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
            Debug.WriteLine($"[FMC TOKEN] {CrossFirebasePushNotification.Current.Token}");
        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            Debug.WriteLine($"[FMC TOKEN UPDATE] {e.Token}");
        }

        /// <summary>
        /// Metodo a ejecutar cuando la app entra en estado de hibernacion
        /// </summary>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        /// <summary>
        /// Metodo a ejecutar cuando la app entra en resumen
        /// </summary>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        /// <summary>
        /// Reinicia la app en la pagina de login
        /// </summary>
        public void Restart()
        {
            SetNavigationPage(new LoginPage());
        }

        /// <summary>
        /// Asigna una navigation page como inicio
        /// </summary>
        /// <param name="page"></param>
        public void SetNavigationPage(Page page)
        {
            var set = new NavigationPage(page)
            {
                BarBackgroundColor = (Color) Current.Resources["Accent"],
                BarTextColor = Color.White,
            };
            SetMainPage(set);
        }

        /// <summary>
        /// Asigna una pagina cualquiera a mainpage
        /// MainPage = page;
        /// </summary>
        /// <param name="page"></param>
        public void SetMainPage(Page page)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                MainPage = page;
            });
        }
    }
}
