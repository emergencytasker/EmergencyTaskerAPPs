using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using Plugin.Net.Socket;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class HamburguerMenuViewModel : ViewModelBase
    {

        #region Notified Property UserImage
        /// <summary>
        /// UserImage
        /// </summary>
        private ImageSource userimage;
        public ImageSource UserImage
        {
            get { return userimage; }
            set { userimage = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property UserName
        /// <summary>
        /// UserName
        /// </summary>
        private string username;
        public string UserName
        {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Review
        /// <summary>
        /// Review de la propiedad bindable
        /// </summary>
        private string review;
        public string Review
        {
            get { return review; }
            set { review = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Balance
        /// <summary>
        /// Balance
        /// </summary>
        private string balance;
        public string Balance
        {
            get { return balance; }
            set { balance = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapProfile
        /// <summary>
        /// TapProfile de la propiedad bindable
        /// </summary>
        private Command tapprofile;
        public Command TapProfile
        {
            get { return tapprofile; }
            set { tapprofile = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty MenuItem
        /// <summary>
        /// MenuItem de la propiedad bindable
        /// </summary>
        private HamburguerMenuItem menuitem;
        public HamburguerMenuItem MenuItem
        {
            get { return menuitem; }
            set { menuitem = value; OnPropertyChanged(); if (value != null) { MenuItemAction(value); } }
        }

        private void MenuItemAction(HamburguerMenuItem item)
        {
            if (item == null) return;
            var targettype = item.Page;
            Page page = null;
            if (item.PageArgs != null && item.PageArgs.Length > 0)
            {
                page = (Page) Activator.CreateInstance(targettype, item.PageArgs);
            }
            else
            {
                page = (Page) Activator.CreateInstance(targettype);
            }

            if (page != null)
            {
                page.Title = item.Title;
                if (item.ViewModel != null)
                {
                    try
                    {
                        object viewmodel = null;
                        if (item.ViewModelArgs != null && item.ViewModelArgs.Length > 0)
                        {
                            viewmodel = Activator.CreateInstance(item.ViewModel, item.ViewModelArgs);
                        }
                        else
                        {
                            viewmodel = Activator.CreateInstance(item.ViewModel);
                        }
                        page.BindingContext = viewmodel;
                    }
                    catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.StackTrace); }
                    
                }

                SetDetail(page);
            }
        }
        #endregion

        public MasterDetailPage MasterDetailPage { get; set; }
        public ObservableCollection<HamburguerMenuItem> MenuItems { get; set; }

        public HamburguerMenuViewModel()
        {
            MenuItems = new ObservableCollection<HamburguerMenuItem>();
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            UserImage = Client.GetPath(usuario.imagen);
            UserName = usuario.nombre;
            var balance = await Client.GetBalance(usuario.id);
            Balance = $"${balance}";
            Review = (await Client.GetReview(usuario.id)).ToString();
            var perfil = (Perfil) usuario.idperfil;
            switch (perfil)
            {
                case Perfil.Client:
                    MenuParaContratista();
                    break;
                case Perfil.Tasker:
                    MenuParaAsistente();
                    break;
            }

            TapProfile = new Command(TapProfile_Clicked);
        }

        private void MenuParaAsistente()
        {
            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Inicio,
                Page = typeof(WaitingServicePage),
                ViewModel = typeof(WaitingServiceViewModel),
                Image = "\ue88a",
                Badge = 0
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Perfil,
                Page = typeof(ProfilePage),
                ViewModel = typeof(ProfileViewModel),
                Image = "\ue7fd"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Calendario,
                Page = typeof(CalendarPage),
                ViewModel = typeof(CalendarViewModel),
                Image = "\ue616"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Tiempo,
                Page = typeof(ScheduleListPage),
                ViewModel = typeof(ScheduleListViewModel),
                Image = "\ue01b"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Chat,
                Page = typeof(ChatListPage),
                Image = "\ue8af",
                ViewModel = typeof(ChatListViewModel)
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Recompensas,
                Page = typeof(RedeemListPage),
                ViewModel = typeof(RedeemListViewModel),
                Image = "\ue53f"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Notificaciones,
                Page = typeof(NotificationPage),
                ViewModel = typeof(NotificationViewModel),
                Image = "\ue7f4"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = (Application.Current as App).Perfil == Perfil.Client ? AppResource.FormasPago : AppResource.CobroDeServicios,
                Page = typeof(PaymentPage),
                ViewModel = typeof(PaymentViewModel),
                Image = "\ue870"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.CerrarSesion,
                Page = typeof(LogoutPage),
                ViewModel = typeof(LogoutViewModel),
                Image = "\ue8c6",
                Badge = 0
            });

#if DEBUG
            MenuItems.Add(new HamburguerMenuItem
            {
                Title = "Globals",
                Page = typeof(JsonPage),
                ViewModel = typeof(JsonViewModel),
                Image = "\ue001",
                Badge = 0
            });
#endif
        }

        private void MenuParaContratista()
        {
            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Inicio,
                Page = typeof(HomePage),
                ViewModel = typeof(HomeViewModel),
                Image = "\ue88a",
                Badge = 0
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Perfil,
                Page = typeof(ProfileClientPage),
                ViewModel = typeof(ProfileClientViewModel),
                Image = "\ue7fd"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Calendario,
                Page = typeof(CalendarPage),
                ViewModel = typeof(CalendarViewModel),
                Image = "\ue616"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Chat,
                Page = typeof(ChatListPage),
                Image = "\ue8af",
                ViewModel = typeof(ChatListViewModel)
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Recompensas,
                Page = typeof(RedeemListPage),
                ViewModel = typeof(RedeemListViewModel),
                Image = "\ue53f"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.Notificaciones,
                Page = typeof(NotificationPage),
                ViewModel = typeof(NotificationViewModel),
                Image = "\ue7f4"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.FormasPago,
                Page = typeof(PaymentPage),
                ViewModel = typeof(PaymentViewModel),
                Image = "\ue870"
            });

            MenuItems.Add(new HamburguerMenuItem
            {
                Title = AppResource.CerrarSesion,
                Page = typeof(LogoutPage),
                ViewModel = typeof(LogoutViewModel),
                Image = "\ue8c6",
                Badge = 0
            });

#if DEBUG
            MenuItems.Add(new HamburguerMenuItem
            {
                Title = "Globals",
                Page = typeof(JsonPage),
                ViewModel = typeof(JsonViewModel),
                Image = "\ue001",
                Badge = 0
            });
#endif
        }

        private async void TapProfile_Clicked(object obj)
        {
            if(App.Perfil == Perfil.Client)
            {
                await Navigation.PushAsync(new ProfileClientPage());
            }
            else
            {
                await Navigation.PushAsync(new ProfilePage());
            }
        }

        public void SetMasterDetailPage(MasterDetailPage page)
        {
            MasterDetailPage = page;
        }

        private void SetDetail(Page page)
        {
            if (MasterDetailPage == null) return;
            if (page == null) return;
            MasterDetailPage.Detail = new NavigationPage(page)
            {
                BarBackgroundColor = (Color)App.Resources["Accent"],
                BarTextColor = Color.White
            };
            MasterDetailPage.IsPresented = false;
        }
    }
}
