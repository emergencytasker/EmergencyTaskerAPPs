namespace EmergencyTask.Model
{
    public class FilterModel : ModelBase
    {
        #region Notified Property Calificacion
        /// <summary>
        /// Calificacion
        /// </summary>
        private bool calificacion;
        public bool Calificacion
        {
            get { return calificacion; }
            set { calificacion = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Tareas
        /// <summary>
        /// Tareas
        /// </summary>
        private bool tareas;
        public bool Tareas
        {
            get { return tareas; }
            set { tareas = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Distancia
        /// <summary>
        /// Distancia
        /// </summary>
        private int distancia;
        public int Distancia
        {
            get { return distancia; }
            set { distancia = value; OnPropertyChanged(); }
        }
        #endregion

    }
}