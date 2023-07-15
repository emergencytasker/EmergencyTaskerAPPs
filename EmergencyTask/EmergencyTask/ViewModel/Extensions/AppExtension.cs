using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel.Extensions
{
    public static class AppExtension
    {
        /// <summary>
        /// Almacena la ubicacion actual
        /// </summary>
        public static Location CurrentLocation { get; set; }

        /// <summary>
        /// Devuelve la distancia de un usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="distanceunits"></param>
        /// <returns></returns>
        public static async Task<double> DistanceTo(this User user, bool refreshlocation = false, DistanceUnits distanceunits = DistanceUnits.Miles)
        {
            if (CurrentLocation == null || refreshlocation)
            {
                try
                {
                    CurrentLocation = await Geolocation.GetLastKnownLocationAsync();
                }
                catch { }
            }
            if (CurrentLocation == null) return 0;
            return Math.Round(CurrentLocation.CalculateDistance(user.latitud, user.longitud, distanceunits), 2);
        }

        /// <summary>
        /// Revisa si hay una forma de pago para el usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<bool> HasPaymethod(this Usuario user)
        {
            var customer = await user.GetCustomerId();
            if (string.IsNullOrEmpty(customer)) return false;
            var stripe = await (Application.Current as App).GetStripeAsync();
            if (stripe == null) return false;
            var paymethod = await stripe.GetCustomerPaymethod(customer);
            if (paymethod == null) return false;
            return true;
        }

        /// <summary>
        /// Devuelve el customer id del usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<string> GetCustomerId(this Usuario user)
        {
            var userstripe = (await Client.Stripeuser.Where(new Stripeuser
            {
                idusuario = user.id
            })).FirstOrDefault();
            if (userstripe == null) return string.Empty;
            return userstripe.customer;
        }

        /// <summary>
        /// Devuelve una variable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetVar<T>(this App app, string key)
        {
            return await Client.GetVar<T>(key, (int)app.Perfil);
        }

        /// <summary>
        /// Obtiene la configuracion para stripe
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static async Task<API.Stripe> GetStripeAsync(this App app)
        {
            var pkey = await app.GetVar<string>("pkeystripe");
            var skey = await app.GetVar<string>("skeystripe");
            var modo = await app.GetVar<string>("modo");
            if (string.IsNullOrEmpty(pkey) || string.IsNullOrEmpty(skey) || string.IsNullOrEmpty(modo)) return null;
            API.Stripe client = new API.Stripe(pkey, skey);
            return client;
        }

        /// <summary>
        /// Realiza el pago de un hito
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="amount"></param>
        /// <param name="description"></param>
        /// <param name="idhito"></param>
        /// <returns></returns>
        public static async Task<string> AuthorizeHito(this Usuario usuario, double amount, string description, int idhito)
        {
            var date = await Client.GetDate();
            if (date == null) return string.Empty;

            var now = date.Value;

            var customer = await usuario.GetCustomerId();
            var stripe = await (Application.Current as App).GetStripeAsync();
            if (stripe == null || string.IsNullOrEmpty(customer)) return string.Empty;
            var price = (amount * 100).ToString();
            long.TryParse(price, out long stripeprice);
            if (stripeprice <= 0) return string.Empty;
            var chargeid = await stripe.CreateCharge(stripeprice, "usd", customer, description, usuario.email);
            if (string.IsNullOrEmpty(chargeid)) return string.Empty;
            var fechaautorizacion = now.ToMySqlDateTimeFormat();
            var hitostatus = (int) HitoStatus.AuthorizedFunds;
            var hitoupdate = await Client.Hito.Update(idhito, new Dictionary<string, string>
            {
                { nameof(Hito.chargeid), chargeid },
                { nameof(Hito.estado), hitostatus.ToString() },
                { nameof(Hito.fechadeautorizacion), fechaautorizacion }
            });
            if(hitoupdate != null && hitoupdate.estado == hitostatus && hitoupdate.fechadeautorizacion == fechaautorizacion) return chargeid;
            return chargeid;
        }

        /// <summary>
        /// Realiza la liberacion del hito
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="amount"></param>
        /// <param name="description"></param>
        /// <param name="idhito"></param>
        /// <returns></returns>
        public static async Task<bool> ReleaseHito(this Usuario usuario, string chargeid, double amount, int? idhito, bool appfee)
        {
            var customer = await usuario.GetCustomerId();
            var stripe = await (Application.Current as App).GetStripeAsync();
            if (stripe == null || string.IsNullOrEmpty(customer)) return false;

            var price = (amount * 100).ToString();
            long.TryParse(price, out long stripeprice);

            if (stripeprice <= 0) return false;

            var status = await stripe.CaptureCharge(chargeid, stripeprice);
            if (!status) return false;

            if (!idhito.HasValue) return true;

            await TransferToBalance(idhito.Value, amount, appfee);

            return true;
        }

        public static async Task<bool> TransferToBalance(int idhito, double amount, bool appfee)
        {
            var date = await Client.GetDate();
            if (date == null) return false;
            var now = date.Value;
            var fechadeliberacion = now.ToMySqlDateTimeFormat();
            var hitostatus = (int)HitoStatus.ReleaseFunds;
            var cantidadtasker = amount;
            var cantidadapp = 0D;
            if (appfee)
            {
                var percent = 50D; // porcentaje desde la base de datos
                var apppercent = (percent / 100D); // porcentaje desde la base de datos
                var taskerpercent = ((100D - percent) / 100D);
                cantidadtasker = amount * taskerpercent;
                cantidadapp = amount * apppercent;
            }
            var hitoupdate = await Client.Hito.Update(idhito, new Dictionary<string, string>
            {
                { nameof(Hito.estado), hitostatus.ToString() },
                { nameof(Hito.fechadeliberacion), fechadeliberacion },
                { nameof(Hito.cantidadtrabajador), cantidadtasker.ToString() },
                { nameof(Hito.cantidademergencytasker), cantidadapp.ToString() },
                { nameof(Hito.costofinal), amount.ToString() }
            });

            if (hitoupdate == null || hitoupdate.estado != hitostatus || hitoupdate.fechadeliberacion != fechadeliberacion) return false;

            var saldo = (await Client.Balance.Where(new Balance
            {
                idhito = idhito
            })).FirstOrDefault();

            if (saldo == null)
            {
                saldo = await Client.Balance.Add(new Balance
                {
                    idhito = hitoupdate.id,
                    idperfil = (int)Perfil.Tasker,
                    idusuario = hitoupdate.trabajador,
                    cantidad = cantidadtasker,
                    descripcion = hitoupdate.descripcion
                });
            }

            return saldo != null && saldo.id > 0;
        }

        /// <summary>
        /// Solicita un refund del hito
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="idhito"></param>
        /// <returns></returns>
        public static async Task<bool> RequestRefundHito(this Usuario usuario, int idhito)
        {
            var date = await Client.GetDate();
            if (date == null) return false;
            var now = date.Value;
            var fechasolicitudreembolso = now.ToMySqlDateTimeFormat();
            var hitostatus = (int)HitoStatus.RequestRefund;
            var requestrefund = await Client.Hito.Update(idhito, new Dictionary<string, string>
            {
                { nameof(Hito.estado), hitostatus.ToString() },
                { nameof(Hito.fechasolicutudreembolso), fechasolicitudreembolso }
            });
            if (requestrefund == null || requestrefund.estado != hitostatus) return false;
            return true;
        }
    }
}