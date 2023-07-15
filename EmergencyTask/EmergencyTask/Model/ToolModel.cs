using EmergencyTask.ViewModel.Commands;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class ToolModel : ModelBase
    {

        public int Id { get; set; }

        #region Notified Property Nombre
        /// <summary>
        /// Nombre
        /// </summary>
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Costo
        /// <summary>
        /// Costo
        /// </summary>
        private string costo;
        public string Costo
        {
            get { return costo; }
            set { costo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Cantidad
        /// <summary>
        /// Cantidad
        /// </summary>
        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsActionVisible
        /// <summary>
        /// IsActionVisible
        /// </summary>
        private bool isactionvisible;
        public bool IsActionVisible
        {
            get { return isactionvisible; }
            set { isactionvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsSubtitleEntryVisible
        /// <summary>
        /// IsSubtitleEntryVisible
        /// </summary>
        private bool issubtitleentryvisible;
        public bool IsSubtitleEntryVisible
        {
            get { return issubtitleentryvisible; }
            set { issubtitleentryvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsSubtitleLabelVisible
        /// <summary>
        /// IsSubtitleLabelVisible
        /// </summary>
        private bool issubtitlelabelvisible;
        public bool IsSubtitleLabelVisible
        {
            get { return issubtitlelabelvisible; }
            set { issubtitlelabelvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Action
        /// <summary>
        /// Action
        /// </summary>
        private ExtendCommand action;
        public ExtendCommand Action
        {
            get { return action; }
            set { action = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Command
        /// <summary>
        /// Command
        /// </summary>
        private Command command;
        public Command Command
        {
            get { return command; }
            set { command = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Update
        /// <summary>
        /// Update
        /// </summary>
        private Command update;
        public Command Update
        {
            get { return update; }
            set { update = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Delete
        /// <summary>
        /// Delete
        /// </summary>
        private ICommand delete;
        public ICommand Delete
        {
            get { return delete; }
            set { delete = value; OnPropertyChanged(); }
        }
        #endregion
    }

}