namespace EmergencyTask.Model
{
    public class CommonModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Categoria { get; internal set; }
        public double Costo { get; internal set; }
    }
}