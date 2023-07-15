using ETClient.API.ER;
using ETClient.Models;
using GoogleApi.Entities.Search.Common.Enums;
using System.Drawing;

namespace ETClient
{
    public static class AppExtension
    {
        public static async Task<double> DistanceTo(this User user, bool refreshlocation = false, DistanceUnits distanceunits = DistanceUnits.Miles)
        {
            return 50;
           // return Math.Round(CurrentLocation.CalculateDistance(user.latitud, user.longitud, distanceunits), 2);
        }

        public static async Task<bool> HasPaymethod(this Usuario user)
        {
            var customer = await user.GetCustomerId();
            if (string.IsNullOrEmpty(customer)) return false;
            var stripe = await App.GetStripeAsync();
            if (stripe == null) return false;
            var paymethod = await stripe.GetCustomerPaymethod(customer);
            if (paymethod == null) return false;
            return true;
        }

		public static async Task<bool> HasPaymethod(this User user)
		{
			var customer = await user.GetCustomerId();
			if (string.IsNullOrEmpty(customer)) return false;
			var stripe = await App.GetStripeAsync();
			if (stripe == null) return false;
			var paymethod = await stripe.GetCustomerPaymethod(customer);
			if (paymethod == null) return false;
			return true;
		}

		public static async Task<string> GetCustomerId(this Usuario user)
        {
            var userstripe = (await Client.Stripeuser.Where(new Stripeuser
            {
                idusuario = user.id
            })).FirstOrDefault();
            if (userstripe == null) return string.Empty;
            return userstripe.customer;
        }
		public static async Task<string> GetCustomerId(this User user)
		{
			var userstripe = (await Client.Stripeuser.Where(new Stripeuser
			{
				idusuario = user.id
			})).FirstOrDefault();
			if (userstripe == null) return string.Empty;
			return userstripe.customer;
		}

        public static string ToHex(this Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }
    }

}
