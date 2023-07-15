using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Strings;
using Plugin.UI.Xaml.Calendar;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace EmergencyTask.Model
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
            Status = AppResource.ResourceManager.GetString(estado.ToString());
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
                    return (Color)App.Current.Resources["Accent"];
                case EstadoServicio.TrabajoIncompleto:
                    return Color.DarkRed;
                case EstadoServicio.Calificado:
                    return Color.DarkGoldenrod;
                case EstadoServicio.Finalizado:
                    return (Color)App.Current.Resources["Accent"];
            }
            return Color.Orange;
        }
    }
}