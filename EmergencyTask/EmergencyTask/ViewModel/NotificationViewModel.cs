using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using EmergencyTask.Strings;
using EmergencyTask.Helpers;
using EmergencyTask.API.Enum;
using EmergencyTask.ViewModel.Business;
using Newtonsoft.Json;

namespace EmergencyTask.ViewModel
{
    public class NotificationViewModel : ViewModelBase
    {

        #region BindableProperty Carta
        /// <summary>
        /// Carta de la propiedad bindable
        /// </summary>
        private NotificationModel notification;
        public NotificationModel Notification
        {
            get { return notification; }
            set { notification = value; OnPropertyChanged(); OnNotificationSelected(value); }
        }
        #endregion

        #region BindableProperty Cartas
        /// <summary>
        /// Cartas de la propiedad bindable
        /// </summary>
        private ObservableCollection<NotificationModel> notifications;
        public ObservableCollection<NotificationModel> Notifications
        {
            get { return notifications; }
            set { notifications = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Refresh
        /// <summary>
        /// Refresh
        /// </summary>
        private Command refresh;
        public Command Refresh
        {
            get { return refresh; }
            set { refresh = value; OnPropertyChanged(); }
        }
        #endregion

        public IEnumerable<Notification> ApiNotifications { get; set; }

        public NotificationViewModel()
        {
        }

        private async void OnNotificationSelected(NotificationModel value)
        {
            try
            {
                if (value == null) return;
                NotificationData data = null;
                data = JsonConvert.DeserializeObject<NotificationData>(value.Data);
                if (data == null) return;
                var action = (NotificationActions)value.IdAction;
                var factory = new EnumFactory<NotificationActions, INotificationActionHandler>("EmergencyTask.ViewModel.Business.", "Handler");
                if (factory == null) return;
                var handler = factory.Resolve(action);
                if (handler == null) return;
                await handler?.Execute(value, data);
                Notification = null;
            }
            catch { }
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            Refresh = new Command(Refresh_Command);
            await Load();
        }

        private async void Refresh_Command(object obj)
        {
            await Load();
        }

        private async Task Load()
        {
            if (IsBusy) return;
            IsBusy = true;

            var nowdb = await Client.GetDate();
            if (nowdb == null)
            {
                Toast(AppResource.SinConfigurarHora);
                return;
            }
            var now = nowdb.Value;

            var usuario = Usuario.GetUserLogin();
            if (usuario == null)
            {
                IsBusy = false;
                return;
            }

            ApiNotifications = (await Client.Notification.Where(new Notification
            {
                idusuario = usuario.id
            }));

            var notifications = ApiNotifications.OrderByDescending(n => DateTime.Parse(n.fecha)).Select(n => GetModel(n)).Where(n => n != null);

            SetSource(notifications);

            IsBusy = false;
        }

        private NotificationModel GetModel(Notification n)
        {
            if (n == null) return null;
            var horafecha = AppResource.JustoAhora;
            if (DateTime.TryParse(n.fecha, out DateTime notificationdate))
            {
                horafecha = notificationdate.ToPrettyDate();
            }
            NotificationData data = null;
            try
            {
                data = JsonConvert.DeserializeObject<NotificationData>(n.data);
            }
            catch { return null; }
            var id = data == null ? n.id : data.id;
            if(id == 0) return null;
            return new NotificationModel
            {
                Title = n.title,
                Subtitle = n.message,
                HoraFecha = horafecha,
                Id = id,
                IdAction = n.idaction,
                Data = n.data
            };
        }

        private void SetSource(IEnumerable<NotificationModel> notifications)
        {
            Notifications = new ObservableCollection<NotificationModel>(notifications);
        }
    }
}
