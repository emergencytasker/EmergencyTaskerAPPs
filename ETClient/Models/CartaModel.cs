using ETClient.Commands;
using System.Windows.Input;

namespace ETClient.Models
{
    public class CartaModel : ModelBase
    {

        #region BindableProperty Image
        /// <summary>
        /// Image de la propiedad bindable
        /// </summary>
        private string image;
        public string Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Subtitle
        /// <summary>
        /// Subtitle de la propiedad bindable
        /// </summary>
        private string subtitle;
        public string Subtitle
        {
            get { return subtitle; }
            set { subtitle = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Action
        /// <summary>
        /// Action de la propiedad bindable
        /// </summary>
        private ExtendCommand action;
        public ExtendCommand Action
        {
            get { return action; }
            set { action = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Title
        /// <summary>
        /// Title de la propiedad bindable
        /// </summary>
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        public int Id { get; internal set; }
        #endregion

        #region BindableProperty Asistente
        /// <summary>
        /// Asistente de la propiedad bindable
        /// </summary>
        private string asistente;
        public string Asistente
        {
            get { return asistente; }
            set { asistente = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty HoraFecha
        /// <summary>
        /// HoraFecha de la propiedad bindable
        /// </summary>
        private string name;
        public string HoraFecha
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Costo
        /// <summary>
        /// Costo de la propiedad bindable
        /// </summary>
        private string costo;
        public string Costo
        {
            get { return costo; }
            set { costo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Precio
        /// <summary>
        /// Precio
        /// </summary>
        private double precio;
        public double Precio
        {
            get { return precio; }
            set { precio = value; OnPropertyChanged(); }
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

        #region Notified Property IdCategory
        /// <summary>
        /// IdCategory
        /// </summary>
        private int idcategory;
        public int IdCategory
        {
            get { return idcategory; }
            set { idcategory = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Command
        /// <summary>
        /// Command de la propiedad bindable
        /// </summary>
        private ExtendCommand command;
        public ExtendCommand Command
        {
            get { return command; }
            set { command = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsActionVisible
        /// <summary>
        /// IsActionVisible de la propiedad bindable
        /// </summary>
        private bool isactionvisible;
        public bool IsActionVisible
        {
            get { return isactionvisible; }
            set { isactionvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsSubtitleLabelVisible
        /// <summary>
        /// IsSubtitleLabelVisible de la propiedad bindable
        /// </summary>
        private bool issubtitlelabelvisible;
        public bool IsSubtitleLabelVisible
        {
            get { return issubtitlelabelvisible; }
            set { issubtitlelabelvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsSubtitleEntryVisible
        /// <summary>
        /// IsSubtitleEntryVisible de la propiedad bindable
        /// </summary>
        private bool issubtitlevisible;
        public bool IsSubtitleEntryVisible
        {
            get { return issubtitlevisible; }
            set { issubtitlevisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Update
        /// <summary>
        /// Update
        /// </summary>
        private ExtendCommand update;
        public ExtendCommand Update
        {
            get { return update; }
            set { update = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Delete
        /// <summary>
        /// Delete
        /// </summary>
        private ExtendCommand delete;
        public ExtendCommand Delete
        {
            get { return delete; }
            set { delete = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Taskers
        /// <summary>
        /// Taskers
        /// </summary>
        private string taskers;
        public string Taskers
        {
            get { return taskers; }
            set { taskers = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property TapMenu
        /// <summary>
        /// TapMenu
        /// </summary>
        private ExtendCommand tapmenu;
        public ExtendCommand TapMenu
        {
            get { return tapmenu; }
            set { tapmenu = value; OnPropertyChanged(); }
        }
        #endregion

    }
}
