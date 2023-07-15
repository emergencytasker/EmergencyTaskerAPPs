using ETClient.API.ER;
using ETClient.Commands;
using System.Windows.Input;

namespace ETClient.Models
{
    public class CandidateModel : ModelBase
    {

        #region BindableProperty BtnContratar
        /// <summary>
        /// BtnContratar de la propiedad bindable
        /// </summary>
        private ExtendCommand btncontratar;
        public ExtendCommand BtnContratar
        {
            get { return btncontratar; }
            set { btncontratar = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapMessage
        /// <summary>
        /// TapMessage de la propiedad bindable
        /// </summary>
        private ExtendCommand tapmessage;
        public ExtendCommand TapMessage
        {
            get { return tapmessage; }
            set { tapmessage = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapProfile
        /// <summary>
        /// TapProfile de la propiedad bindable
        /// </summary>
        private ExtendCommand tapprofile;
        public ExtendCommand TapProfile
        {
            get { return tapprofile; }
            set { tapprofile = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty NombreAsistente
        /// <summary>
        /// NombreAsistente de la propiedad bindable
        /// </summary>
        private string nombreasistente;
        public string NombreAsistente
        {
            get { return nombreasistente; }
            set { nombreasistente = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty FotoAsistente
        /// <summary>
        /// FotoAsistente de la propiedad bindable
        /// </summary>
        private string fotoasistente;
        public string FotoAsistente
        {
            get { return fotoasistente; }
            set { fotoasistente = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty CostoTime
        /// <summary>
        /// CostoTime de la propiedad bindable
        /// </summary>
        private string costotime;
        public string CostoTime
        {
            get { return costotime; }
            set { costotime = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Review
        /// <summary>
        /// Review de la propiedad bindable
        /// </summary>
        private double review;
        public double Review
        {
            get { return review; }
            set { review = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Task
        /// <summary>
        /// Task de la propiedad bindable
        /// </summary>
        private string task;
        public string Task
        {
            get { return task; }
            set { task = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Distancia
        /// <summary>
        /// Distancia
        /// </summary>
        private double distancia;
        public double Distancia
        {
            get { return distancia; }
            set { distancia = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Tareas
        /// <summary>
        /// Tareas
        /// </summary>
        private long tareas;
        public long Tareas
        {
            get { return tareas; }
            set { tareas = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Calificacion
        /// <summary>
        /// Calificacion
        /// </summary>
        private double calificacion;
        public double Calificacion
        {
            get { return calificacion; }
            set { calificacion = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property TapInfoCosto
        /// <summary>
        /// TapInfoCosto
        /// </summary>
        private ICommand tapinfocosto;
        public ICommand TapInfoCosto
        {
            get { return tapinfocosto; }
            set { tapinfocosto = value; OnPropertyChanged(); }
        }
        #endregion

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int ServiceId { get; set; }
        public double Cost { get; set; }
        public double ElapsedDaysFromLastSession { get; set; }
        public double ElapsedDaysFromLastService { get; set; }

        public CandidateModel(API.Response.Tasker tasker)
        {
            Id = tasker.Id;
            IdUsuario = tasker.Id;
            FotoAsistente = Client.GetPath(tasker.Imagen);
            NombreAsistente = tasker.Nombre;
            Cost = tasker.Costoporhora;
            CostoTime = $"${Math.Round(tasker.Costoporhora, 2)} per hour";
            Review = tasker.Calificacion;
            Task = $"{tasker.Tareascompletas} Tasks";
            Distancia = Math.Round(tasker.DistanceMiles, 2);
            Calificacion = Math.Round(tasker.Calificacion, 1);
            Tareas = tasker.Tareascompletas;
            ServiceId = tasker.Idservicio;
            ElapsedDaysFromLastService = tasker.ElapsedDaysFromLastService;
            ElapsedDaysFromLastSession = tasker.ElapsedDaysFromLastSession;

        }

        public CandidateModel(User user, Service service)
        {
            Id = user.id;
            IdUsuario = user.id;
            FotoAsistente = Client.GetPath(user.imagen);
            NombreAsistente = user.nombre;
            Cost = service.costo;
            CostoTime = $"${Math.Round(service.costo, 2)} per hour";
            Review = user.calificacion;
            Task = $"{user.tareas} Tasks";
            Distancia = 0;
            user.DistanceTo(false, DistanceUnits.Miles).ContinueWith(r =>
            {
                Distancia = Math.Round(r.Result, 2);
            });
            Calificacion = user.calificacion;
            Tareas = user.tareas;
            ServiceId = service.id;
        }
    }
}
