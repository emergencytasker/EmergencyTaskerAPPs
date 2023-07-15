namespace ETClient.Models
{
	public class AccesorioModel : ModelBase
	{

		public string Nombre { get; set; }
		public int Cantidad { get; set; }

		public override bool Validation()
		{
			return !string.IsNullOrEmpty(Nombre) && Cantidad > 0;
		}
	}
}
