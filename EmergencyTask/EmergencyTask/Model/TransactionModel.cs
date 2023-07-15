using System;
using EmergencyTask.Helpers;
using EmergencyTask.ViewModel.Commands;
using Xamarin.Forms;

namespace EmergencyTask.Model
{
    public class TransactionModel : ModelBase
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public int? IdHito { get; }
        public string Fecha { get; set; }
        public double Cantidad { get; set; }
        public Color Color { get; set; }
        public DateTime Time { get; set; }
        public TransactionType TransactionType { get; set; }

        #region Notified Property GoToTransaction
        /// <summary>
        /// GoToTransaction
        /// </summary>
        private ExtendCommand gototransaction;
        public ExtendCommand GoToTransaction
        {
            get { return gototransaction; }
            set { gototransaction = value; OnPropertyChanged(); }
        }
        #endregion

        public TransactionModel(API.ER.Hito hito)
        {
            if (hito == null) return;
            Id = hito.idsolicitudservicio.ToString();
            Descripcion = hito.descripcion;
            IdHito = hito.id;
            var date = hito.fecha.FromMySqlDateTimeFormat();
            Fecha = date.ToLongDateString();
            if(hito.cantidadtrabajador.HasValue && hito.cantidademergencytasker.HasValue)
            {
                if((Application.Current as App).Perfil == API.Enum.Perfil.Client)
                {
                    Cantidad = hito.costofinal;
                }
                else
                {
                    Cantidad = hito.cantidadtrabajador.Value;
                }
            }
            else
            {
                Cantidad = hito.cantidad;
            }
            
            Color = hito.cantidad >= 0 ? Color.DarkGreen : Color.DarkRed;
            Time = date;
        }

        public TransactionModel(API.ER.Balance balance)
        {
            if (balance == null) return;
            Id = balance.id.ToString();
            Descripcion = balance.descripcion;
            IdHito = balance.idhito;
            var date = balance.fecha.FromMySqlDateTimeFormat();
            Fecha = date.ToLongDateString();
            Cantidad = balance.cantidad;
            Color = balance.cantidad >= 0 ? Color.DarkGreen : Color.DarkRed;
            Time = date;
            if(balance.essolicitud == 1)
                TransactionType = TransactionType.Payout;
            else if(balance.idhito.HasValue)
                TransactionType = TransactionType.Hito;
            else
                TransactionType = TransactionType.Balance;
        }

        public TransactionModel(Stripe.Charge charge)
        {
            Id = charge.Id;
            Descripcion = charge.Description;
            Fecha = charge.Created.ToLongDateString();
            var amount = (charge.Amount / 100D) * -1;
            Cantidad = amount;
            Color = amount >= 0 ? Color.DarkGreen : Color.DarkRed;
            Time = charge.Created;
            TransactionType = TransactionType.Stripe;
        }
    }
}
