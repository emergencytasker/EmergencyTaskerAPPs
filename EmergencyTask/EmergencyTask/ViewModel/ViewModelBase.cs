using Acr.UserDialogs;
using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using Plugin.Media;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using EmergencyTask.ViewModel.Commands;
using System.Collections.Generic;
using EmergencyTask.Strings;
using Plugin.Notification;
using System.Threading;
using EmergencyTask.View;
using System.Diagnostics;

namespace EmergencyTask.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged, IExtendCommandEvents
    {
        public ViewModelBase()
        {
            App.PageAppearing += App_PageAppearing;
            App.PageDisappearing += App_PageDisappearing;
            MessagingCenter.Subscribe<App, bool>(this, "Busy", (app, state) =>
            {
                IsBusy = state;
            });
        }

        public async Task<Plugin.Media.Abstractions.MediaFile> TakePhoto()
        {
            try
            {
                if (CrossMedia.Current.IsTakePhotoSupported)
                {
                    var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        MaxWidthHeight = 1280,
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                        SaveMetaData = true,
                        CustomPhotoSize = 50
                    });
                    return photo;
                }
            }
            catch { }
            return null;
        }

        public async Task<Plugin.Media.Abstractions.MediaFile> PickPhoto()
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    var photo = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        MaxWidthHeight = 1280,
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                        SaveMetaData = true,
                        CustomPhotoSize = 50
                    });
                    return photo;
                }
            }
            catch { }
            return null;
        }

        #region GeoLocation

        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public Exception LocationException { get; set; }
        public async Task GetLocation(Action<Location> success = null, Action<Exception> fail  = null)
        {
            var me = Usuario.GetUserLogin();
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.High
                });
                if (location != null)
                {
                    Latitud = location.Latitude;
                    Longitud = location.Longitude;
                    if (Latitud == 0 || Longitud == 0) throw new Exception();
                    success?.Invoke(location);
                    if (me == null) return;
                    if (location.Accuracy.HasValue && location.Accuracy.Value > 30) return;
                    await Client.User.Update(me.id, new Dictionary<string, string>
                    {
                        { nameof(User.latitud), Latitud.ToString() },
                        { nameof(User.longitud), Longitud.ToString() }
                    });
                    me.latitud = Latitud;
                    me.longitud = Longitud;
                    Debug.WriteLine($"[POSITION] {Latitud}, {Longitud}");
                    Usuario.SetUserLogin(me);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                LocationException = fnsEx;
                fail?.Invoke(fnsEx);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                LocationException = fneEx;
                fail?.Invoke(fneEx);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                LocationException = pEx;
                fail?.Invoke(pEx);
            }
            catch (Exception ex)
            {
                // Unable to get location
                LocationException = ex;
                fail?.Invoke(ex);
            }
            finally
            {
                if (Latitud == 0D || Longitud == 0D)
                {
                    Latitud = (double)Application.Current.Resources["Latitud"];
                    Longitud = (double)Application.Current.Resources["Longitud"];
                }
            }
        }

        #endregion

        private void App_PageDisappearing(object sender, Page e)
        {
            App.PageDisappearing -= App_PageDisappearing;
            OnDisappearing(e);
        }

        private void App_PageAppearing(object sender, Page e)
        {
            App.PageAppearing -= App_PageAppearing;
            OnAppearing(e);
        }

        /// <summary>
        /// Obtiene los siguientes datos
        /// • Ubicacion
        /// • Notificaciones
        /// </summary>
        /// <param name="page"></param>
        public virtual async void OnAppearing(Page page)
        {
            Debug.WriteLine($"[PAGE] {page.Title}");
            var usuario = Usuario.GetUserLogin();
            if(usuario != null)
            {
                if (usuario.id <= 0)
                {
                    Toast("Tu sesion no es valida");
                    App.Restart();
                    return;
                }
            }
            await GetLocation();
            await GetNotifications();
        }

        public async Task GetNotifications()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            NotificationHelper.Stop();
            var notifications = (await Client.Notification.Where(new Notification
            {
                idusuario = usuario.id
            })).Where(n => n.notificado == 0);
            foreach (var item in notifications)
            {
                var update = await Client.Notification.Update(item.id, new Dictionary<string, string>
                {
                    { nameof(Notification.notificado), "1" }
                });
                Device.BeginInvokeOnMainThread(() =>
                {
                    var dic = new Dictionary<string, string>();
                    try
                    {
                        dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(item.data);
                    }
                    catch { }
                    NotificationHelper.Show(item.title, item.message, dic);
                });
            }
            NotificationHelper.Start();
        }

        public virtual void OnDisappearing(Page page)
        {

        }

        private const string AppName = "Emergency Tasker";

        private bool isbusy;

        public bool IsBusy
        {
            get { return isbusy; }
            set { isbusy = value; OnPropertyChanged(); }
        }

        public App App
        {
            get
            {
                return Application.Current as App;
            }
        }

        public INavigation Navigation
        {
            get
            {
                var app = Application.Current as App;
                if (app.MainPage is NavigationPage navigation)
                {
                    return new NavigationService(navigation.CurrentPage.Navigation);
                }
                else if (app.MainPage is MasterDetailPage masterdetail)
                {
                    if (masterdetail.Detail is NavigationPage navpage)
                    {
                        return new NavigationService(navpage.CurrentPage.Navigation);
                    }
                    else
                    {
                        return new NavigationService(masterdetail.Detail.Navigation);
                    }
                }
                else if (app.MainPage != null)
                {
                    return new NavigationService(app.MainPage.Navigation);
                }
                else
                {
                    throw new NullReferenceException("INavigation is null");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DisplayAlert(string message, string cancel)
        {
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.Alert(message, AppName, cancel));
        }

        public async Task<bool> Confirm(string message)
        {
            return await Device.InvokeOnMainThreadAsync(async () =>
            {
                return await UserDialogs.Instance.ConfirmAsync(message, null, AppResource.Aceptar, AppResource.Omitir);
            });
        }

        public async Task<string> ActionSheet(string message, string cancel, params string[] options)
        {
            return await Device.InvokeOnMainThreadAsync(async () =>
            {
                return await UserDialogs.Instance.ActionSheetAsync(message, cancel, null, null, options);
            });

        }

        public void Toast(string message)
        {
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.Toast(message, TimeSpan.FromSeconds(3)));
        }

        public Task<T> Promt<T>(string message, string cancel, string accept, InputType type)
        {
            TaskCompletionSource<T> completion = new TaskCompletionSource<T>();
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.Prompt(new PromptConfig
            {
                CancelText = cancel,
                IsCancellable = true,
                Message = message,
                OkText = accept,
                InputType = type,
                OnAction = new Action<PromptResult>(response =>
                {
                    if (response.Ok)
                    {
                        try
                        {
                            var value = (T)Convert.ChangeType(response.Value, typeof(T));
                            completion.TrySetResult(value);
                        }
                        catch { }
                    }
                    completion.TrySetResult(default(T));
                })
            }));
            return completion.Task;
        }

        /// <summary>
        /// Asigna el menu hamburguesa de la app
        /// </summary>
        public static async Task OnBasedProfileOpenApp(bool requiredlogin = true)
        {
            var app = Application.Current as App;
            MessagingCenter.Instance.Send(app, "IsBusy", true);
            var usuario = Usuario.GetUserLogin();
            if (usuario == null)
            {
                MessagingCenter.Instance.Send(app, "IsBusy", false);
                return;
            }

            if (requiredlogin)
            {
                var auth = await Client.Auth(usuario.email, usuario.password, Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
                if (auth == null || !auth.status)
                {
                    MessagingCenter.Instance.Send(app, "IsBusy", false);
                    return;
                }
                Client.SetToken(auth.token);
                var user = await Client.User.Get(auth.code);
                if (user == null)
                {
                    MessagingCenter.Instance.Send(app, "IsBusy", false);
                    return;
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                var newuser = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuario>(json);
                newuser.token = auth.token;
                newuser.password = usuario.password;
                Usuario.SetUserLogin(newuser);
            }

            usuario = Usuario.GetUserLogin();
            if (usuario == null)
            {
                MessagingCenter.Instance.Send(app, "IsBusy", false);
                return;
            }

            var appcenterid = DataBase.GetVariable().AppCenterId;
            var canreceivednotifications = await NotificationHelper.RegisterForNotifications(usuario.id, appcenterid);

            var variable = DataBase.GetVariable();
            variable.CanReceivedNotifications = canreceivednotifications;
            DataBase.SetVariable(variable);

            var menu = new HamburgerMenuPage();
            var perfil = (Perfil)usuario.idperfil;

            if (usuario.Perfil == app.Perfil)
            {
                switch (perfil)
                {
                    case Perfil.Client:
                        menu.Detail = new NavigationPage(new HomePage())
                        {
                            Title = AppResource.Home,
                            BarBackgroundColor = (Color)Application.Current.Resources["Accent"],
                            BarTextColor = Color.White
                        };
                        break;
                    case Perfil.Tasker:
                        menu.Detail = new NavigationPage(new WaitingServicePage
                        {
                           BindingContext = new WaitingServiceViewModel()
                        })
                        {
                            Title = AppResource.Home,
                            BarBackgroundColor = (Color)Application.Current.Resources["Accent"],
                            BarTextColor = Color.White
                        };
                        break;
                }
                app.SetMainPage(menu);
            }
            else
            {
                var currentapp = app.Perfil.ToString();
                var yourprofile = usuario.Perfil.ToString();
                UserDialogs.Instance.Toast(string.Format(AppResource.IniciaSesionApp, yourprofile, currentapp));
            }

            MessagingCenter.Instance.Send(app, "IsBusy", false);
        }

        /// <summary>
        /// Devuelve una variable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetVar<T>(string key)
        {
            return await Client.GetVar<T>(key, (int)App.Perfil);
        }

        /// <summary>
        /// Cliente: Pagina del mapa con las acciones pendientes del servicio
        /// Trabajador: Pagina del mapa con las acciones a realizar para el servicio
        /// </summary>
        /// <param name="idrequestservice"></param>
        /// <returns></returns>
        public async Task GoToServiceDetailPage(int idrequestservice)
        {
            if (idrequestservice <= 0) return;

            var requestservice = await Client.Requestservice.Get(idrequestservice);
            if (requestservice == null) return;
            ContentPage page;

            if (App.Perfil == Perfil.Client)
            {
                if (requestservice.idestadoservicio == (int)EstadoServicio.TrabajoIniciado)
                {
                    await GoToStartPage(requestservice.id);
                    return;
                }
                else if (requestservice.idestadoservicio == (int)EstadoServicio.TrabajoTerminado)
                {
                    await GoToReview(requestservice.id);
                    return;
                }
                else
                {
                    page = new ServiceMapPage
                    {
                        BindingContext = new ServiceMapVieWModel
                        {
                            CurrentService = requestservice
                        }
                    };
                }
            }
            else
            {
                if (requestservice.idestadoservicio == (int)EstadoServicio.TrabajoIniciado)
                {
                    await GoToStartPage(requestservice.id);
                    return;
                }
                else if (requestservice.idestadoservicio == (int)EstadoServicio.TrabajoTerminado)
                {
                    await GoToReview(requestservice.id);
                    return;
                }
                else
                {
                    Debug.WriteLine($"[GoToServiceDetailPage] {requestservice.id}");
                    page = new WaitingServicePage
                    {
                        BindingContext = new WaitingServiceViewModel(requestservice)
                    };
                }
            }

            SetDetailPage(page);
        }

        /// <summary>
        /// Muestra la informacion del servicio
        /// Con quien
        /// Fecha y hora
        /// Pagos
        /// Calificacion
        /// Ubicacion
        /// </summary>
        /// <param name="idrequestservice"></param>
        /// <returns></returns>
        public async Task GoToServiceInfoPage(int idrequestservice)
        {
            if (idrequestservice <= 0) return;
            await Navigation.PushAsync(new DetailServicePage
            {
                BindingContext = new DetailServiceViewModel(idrequestservice)
            });
        }

        /// <summary>
        /// Navega a la vista de inicio de servicio
        /// </summary>
        /// <param name="requestservice"></param>
        /// <param name="idusuario"></param>
        /// <returns></returns>
        public async Task<bool> GoToStartPage(int idsolicitudservicio)
        {
            if (idsolicitudservicio <= 0) return false;
            var requestservice = await Client.Requestservice.Get(idsolicitudservicio);
            if (requestservice == null) return false;

            if (requestservice.idestadoservicio == (int) EstadoServicio.TrabajoIniciado)
            {
                int idusuario;
                if (App.Perfil == Perfil.Client)
                {
                    // id del trabajador
                    idusuario = requestservice.trabajador;
                }
                else
                {
                    // id del cliente
                    idusuario = requestservice.cliente;
                }

                var user = await Client.User.Get(idusuario);
                if (user == null) return false;
                var service = new Service(requestservice);
                var usermodel = new CandidateModel(user, service);
                var servicemodel = new ServiceModel(requestservice);

                /*
                SetDetailPage(new StartServicePage
                {
                    BindingContext = new StartServiceViewModel(servicemodel, usermodel),
                    Title = $"{AppResource.Servicio} {requestservice.categoria} {requestservice.subcategoria}"
                });
                */

                SetDetailPage(new TimeRegisterPage
                {
                    BindingContext = new TimeRegisterViewModel(servicemodel, usermodel),
                    Title = $"{AppResource.Servicio} {requestservice.categoria} {requestservice.subcategoria}"
                });
                return true;
            }
            else
            {
                await GoToServiceInfoPage(idsolicitudservicio);
            }

            return false;
        }

        public async Task<bool> GoToReview(int idsolicitudservicio)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return false;

            if (idsolicitudservicio <= 0) return false;
            var requestservice = await Client.Requestservice.Get(idsolicitudservicio);
            if (requestservice == null) return false;

            var review = (await Client.Review.Where(new Review
            {
                idperfil = (int)me.Perfil,
                idsolicitudservicio = idsolicitudservicio,
                idusuario = me.id
            })).FirstOrDefault();

            if (review == null)
            {
                SetDetailPage(new ReviewServicePage
                {
                    BindingContext = new ReviewServiceViewModel(requestservice),
                    Title = $"{AppResource.CalificarServicio} • {requestservice.categoria} {requestservice.subcategoria}"
                });
            }
            else
            {
                if (me.Perfil == Perfil.Client)
                {
                    await GoToServiceInfoPage(idsolicitudservicio);
                }
                else if(me.Perfil == Perfil.Tasker)
                {
                    await GoToEvidence(idsolicitudservicio, review.id);
                }
            }
            return true;
        }

        public async Task GoToEvidence(int idsolicitudservicio, int idreview = 0)
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            if (usuario.idperfil == (int)Perfil.Client)
            {
                await GoToServiceInfoPage(idsolicitudservicio);
            }
            else if (usuario.idperfil == (int)Perfil.Tasker)
            {
                if (idreview == 0)
                {
                    var calification = (await Client.Review.Where(new Review
                    {
                        idsolicitudservicio = idsolicitudservicio,
                        idperfil = usuario.idperfil,
                        idusuario = usuario.id
                    })).FirstOrDefault();
                    if (calification == null) return;
                    idreview = calification.id;
                }

                var evidence = new EvidenceListPage
                {
                    Title = AppResource.Evidencias,
                    BindingContext = new EvidenceListViewModel(idsolicitudservicio, idreview)
                };

                var hitos = new HitoListPage
                {
                    Title = AppResource.Pagos,
                    BindingContext = new HitoListViewModel(idsolicitudservicio)
                };

                if(Device.RuntimePlatform == Device.iOS)
                {
                    evidence.IconImageSource = "camera.png";
                    hitos.IconImageSource = "dollar.png";
                }

                var page = new TabbedPage { Title = AppResource.Info };
                page.Children.Add(evidence);
                page.Children.Add(hitos);
                SetDetailPage(page);
            }
        }

        public async Task ShowPendingToRateService()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            Debug.WriteLine($"[ShowPendingToRateService]");
            var servicewaitingforreview = await Client.GetReviewService(usuario.Perfil, usuario.id);
            if (servicewaitingforreview == null) return;
            var review = await Client.Review.Where(new Review
            {
                idsolicitudservicio = servicewaitingforreview.id
            });

            if (review == null || review.Count() == 0) return;

            if(review.Any(r => r.idperfil == usuario.idperfil && r.idusuario == usuario.id))
            {
                return;
            }

            if(!await Confirm(AppResource.ServicioPendienteDeCalificacion)) return;
            await GoToReview(servicewaitingforreview.id);
        }

        public void SetDetailPage(Page page)
        {
            if (page == null) return;
            var menu = new HamburgerMenuPage
            {
                Detail = new NavigationPage(page)
                {
                    Title = page.Title,
                    BarBackgroundColor = (Color)Application.Current.Resources["Accent"],
                    BarTextColor = Color.White
                }
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                App.MainPage = menu;
            });
        }

        /// <summary>
        /// Envia a la home screen del perfil actual de la app
        /// </summary>
        public void GoToProfileHome()
        {
            Page page = null;
            switch (App.Perfil)
            {
                case Perfil.Client:
                    page = new HomePage();
                    break;
                case Perfil.Tasker:
                    page = new WaitingServicePage
                    {
                        BindingContext = new WaitingServiceViewModel()
                    };
                    break;
            }
            SetDetailPage(page);
        }

        public void OnPreExecute()
        {
            IsBusy = true;
        }

        public void OnPostExecute()
        {
            IsBusy = false;
        }
    }
}

