using EmergencyTask.Model;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class StatusViewModel : ViewModelBase
    {

        #region BindableProperty CommandStatus
        /// <summary>
        /// CommandStatus de la propiedad bindable
        /// </summary>
        private Command commandstatus;
        public Command CommandStatus
        {
            get { return commandstatus; }
            set { commandstatus = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty Estado
        /// <summary>
        /// Estado de la propiedad bindable
        /// </summary>
        private StatusModel estado;
        public StatusModel Estado
        {
            get { return estado; }
            set { estado = value; OnPropertyChanged(); if (value != null) { value.Action?.Execute(value); } }
        }
        #endregion


        #region BindableProperty Estados
        /// <summary>
        /// Estados de la propiedad bindable
        /// </summary>
        private ObservableCollection<StatusModel> estados;
        public ObservableCollection<StatusModel> Estados
        {
            get { return estados; }
            set { estados = value; OnPropertyChanged(); }
        }
        #endregion

        public StatusViewModel()
        {
            CommandStatus = new Command(CommandStatus_Clicked);
            Estados = new ObservableCollection<StatusModel>
            {
                new StatusModel
                {
                    Title = "Solicitud Enviada",
                    Fecha = "10/08/19",
                    Hora = "10:58 pm",
                    Status = "Pendiente",
                    Icon = App.Resources["Email"] as string,
                    Action = new Command (Action_Clicked),
                },

                new StatusModel
                {
                    Title = "Solicitud Recibida",
                    Fecha = "10/08/19",
                    Hora = "10:58 pm",
                    Status = "Pendiente",
                    Icon = App.Resources["OpenMail"] as string,
                    Action = new Command (Action_Clicked),
                },

                new StatusModel
                {
                    Title = "Compra de Accesorios",
                    Fecha = "10/08/19",
                    Hora = "10:58 pm",
                    Status = "Pendiente",
                    Icon = App.Resources["Tool"] as string,
                    Action = new Command (Action_Clicked),
                },

                new StatusModel
                {
                    Title = "Camino a Domicilio",
                    Fecha = "10/08/19",
                    Hora = "10:58 pm",
                    Status = "Pendiente",
                    Icon = App.Resources["Run"] as string,
                    Action = new Command (Action_Clicked),
                },

                new StatusModel
                {
                    Title = "Inicio de Tarea",
                    Fecha = "10/08/19",
                    Hora = "10:58 pm",
                    Status = "Pendiente",
                    Icon = App.Resources["Play"] as string,
                    Action = new Command (Action_Clicked),
                },

                new StatusModel
                {
                    Title = "Tarea en Progreso",
                    Fecha = "10/08/19",
                    Hora = "10:58 pm",
                    Status = "Pendiente",
                    Icon = App.Resources["Pause"] as string,
                    Action = new Command (Action_Clicked),
                },

                new StatusModel
                {
                    Title = "Termino de Tarea",
                    Fecha = "10/08/19",
                    Hora = "10:58 pm",
                    Status = "Pendiente",
                    Icon = App.Resources["Stop"] as string,
                    Action = new Command (Action_Clicked),
                },
            };
        }

        private async void CommandStatus_Clicked(object obj)
        {
            await Navigation.PushAsync(new StatusPage());
        }

        private void Action_Clicked(object obj)
        {

        }
    }
}
