namespace EmergencyTask.API.ER
{
    public class Schedule
    {
        public int id { get; set; }
        public int idsubcategoria { get; set; }
        public double costo { get; set; }
        public string inicio { get; set; }
        public string fin { get; set; }
        public int eliminado { get; set; }
        public string tipo { get; set; }
    }
}