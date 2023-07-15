using ETClient.API.ER;
using ETClient.Helpers;

namespace ETClient.Models
{
	public class ServiceModel : ModelBase
	{

		public const string Action = "Action";
		public const string IdKey = "Id";

		public ServiceModel()
		{
			Accesorios = new List<AccesorioModel>();
		}

		public ServiceModel(Requestservice currentService)
		{
			if (currentService == null) throw new NullReferenceException("currentService is null");
			Latitud = currentService.latitud;
			Longitud = currentService.longitud;
			IdCategoria = currentService.idcategoria;
			IdSubcategory = currentService.idsubcategoria;
			Direccion = currentService.direccion;
			Detalles = currentService.detalles;
			HasSchedule = currentService.tienehorario == 1;
			DateTime.TryParse(currentService.fechadeservicio, out DateTime date);
			Time = currentService.tiemposolicitado;
			Date = date;
			Description = currentService.descripcion;
			SubCategory = currentService.subcategoria;
			Category = currentService.categoria;
			CostoPorHora = currentService.costoporhora;
			IdServicio = currentService.idservicio;
			Id = currentService.id;
			Accesorios = new List<AccesorioModel>();
			Start = currentService.fechainicio.FromMySqlDateTimeFormat();
			End = currentService.fechafin.FromMySqlDateTimeFormat();
			Client = currentService.cliente;
			Tasker = currentService.trabajador;
		}

		public int Id { get; set; }
		public double Latitud { get; set; }
		public double Longitud { get; set; }
		public int IdCategoria { get; set; }
		public int IdSubcategory { get; set; }
		public string Direccion { get; set; }
		public string Detalles { get; set; }
		public bool HasSchedule { get; set; }
		public DateTime Date { get; set; }
		public int IdUser { get; set; }
		public string Description { get; set; }
		public string SubCategory { get; set; }
		public string Category { get; set; }
		public double CostoPorHora { get; set; }
		public int IdServicio { get; set; }
		public List<AccesorioModel> Accesorios { get; set; }
		public int Time { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public int Client { get; }
		public int Tasker { get; }
	}
}
