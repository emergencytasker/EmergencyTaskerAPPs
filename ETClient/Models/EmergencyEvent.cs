using ETClient.API.Enum;
using ETClient.API.ER;
using ETClient.Helpers;
using ETClient.Interfaces;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Search.Video.Common;
using GooglePlacesApi.Models;
using System.Drawing;

namespace ETClient.Models
{
    public class EmergencyEvent : Event, IEvent
    {

        public EmergencyEvent(Requestservice service, IEnumerable<Categorylanguage> categories, IEnumerable<Subcategorylanguage> subcategories)
        {
            var category = categories.FirstOrDefault(c => c.idcategoria == service.idcategoria);
            var subcategory = subcategories.FirstOrDefault(c => c.idsubcategoria == service.idsubcategoria);
            var categoryname = category?.traduccion ?? service.categoria;
            var subcategoryname = subcategory?.traduccion ?? service.subcategoria;
            Title = $"{categoryname} • {subcategoryname}";
            Detail = $"{service.direccion}";
            StartDate = service.fechainicio.FromMySqlDateTimeFormat();
            EndDate = service.fechafin.FromMySqlDateTimeFormat();
            Color = GetColor(service);
            var estado = (EstadoServicio)service.idestadoservicio;
            Status = estado.ToString();
            Id = service.id;
        }

        private Color GetColor(Requestservice service)
        {
            var estado = (EstadoServicio)service.idestadoservicio;
            switch (estado)
            {
                case EstadoServicio.Pendiente:
                    return Color.Orange;
                case EstadoServicio.Aceptado:
                    return Color.DarkGreen;
                case EstadoServicio.Cancelado:
                    return Color.DarkRed;
                case EstadoServicio.HerramientasCompradas:
                    return Color.DarkCyan;
                case EstadoServicio.EnCaminoADomicilio:
                    return Color.DarkCyan;
                case EstadoServicio.LlegadaADomicilio:
                    return Color.DarkCyan;
                case EstadoServicio.TrabajoIniciado:
                    return Color.DarkCyan;
                case EstadoServicio.TrabajoTerminado:
                    return Color.FromArgb(52, 122, 235);
                case EstadoServicio.TrabajoIncompleto:
                    return Color.DarkRed;
                case EstadoServicio.Calificado:
                    return Color.DarkGoldenrod;
                case EstadoServicio.Finalizado:
                    return Color.FromArgb(52, 122, 235);
            }
            return Color.Orange;
        }
    }

    public class Event : IEvent
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Color Color { get; set; } = Color.Black;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public Color TitleColor { get; set; } = Color.Black;
        public Color DetailColor { get; set; } = Color.Black;
        public string StartHour
        {
            get { return StartDate.ToShortTimeString(); }
        }
        public string EndHour
        {
            get { return EndDate.AddHours(1).ToShortTimeString(); }
        }

        public string Status { get; set; }
    }
}
