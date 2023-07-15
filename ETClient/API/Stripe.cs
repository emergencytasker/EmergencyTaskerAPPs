using Microsoft.AspNetCore.Cors.Infrastructure;
using Newtonsoft.Json.Linq;
using Stripe;

namespace ETClient.API
{
    public class Stripe
    {
        private string errormessage;
        public string ErrorMessage
        {
            get
            {
                return errormessage;
            }

            set
            {
                errormessage = value;
                ShowMessage();
            }
        }

        private void ShowMessage()
        {
            if (string.IsNullOrEmpty(ErrorMessage)) return;
#if DEBUG
            if (!Error) return;
            try
            {
               // Xamarin.Forms.Application.Current.MainPage.DisplayAlert(AppResource.Info, ErrorMessage, AppResource.Aceptar);
            }
            catch { }
#endif
        }

        public bool Error { get; set; }
        private string SecretKey { get; set; }
        public string PublicKey { get; set; }

        public Stripe(string pkey, string skey)
        {
            PublicKey = pkey;
            SecretKey = skey;
        }

        /// <summary>
        /// Crea un customer
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> CreateCustomer(string name, string description, string token, string email = "", string phone = "")
        {
            ResetError();
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);
                var options = new CustomerCreateOptions
                {
                    Description = description,
                    Name = name,
                    SourceToken = token,
                    Email = email,
                    Phone = phone
                };
                var service = new CustomerService();
                Customer customer = await service.CreateAsync(options);
                var customerid = string.Empty;
                if (customer == null) return customerid;
                customerid = customer.Id;
                return customerid;
            }
            catch (Exception ex)
            {
                Error = true;
                ErrorMessage = ex.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// Crea un token de una tarjeta para poder ser usada en stripe
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cardnumber"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="cvc"></param>
        /// <returns></returns>
        public async Task<string> CreateToken(string name, string cardnumber, long year, long month, string cvc)
        {
            ResetError();
            try
            {
                StripeConfiguration.SetApiKey(PublicKey);
                var tokenOptions = new TokenCreateOptions()
                {
                    Card = new CreditCardOptions
                    {
                        Number = cardnumber,
                        ExpYear = year,
                        ExpMonth = month,
                        Cvc = cvc,
                        Name = name
                    }
                };
                var tokenService = new TokenService();
                Token stripeToken = await tokenService.CreateAsync(tokenOptions);
                if (stripeToken == null) return string.Empty;
                return stripeToken.Id;
            }
            catch (Exception ex)
            {
                Error = true;
                ErrorMessage = ex.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// Actualiza un customer
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> UpdateCustomer(string customerid, string email, string token)
        {
            ResetError();
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);
                var options = new CustomerUpdateOptions
                {
                    SourceToken = token
                };
                var service = new CustomerService();
                var response = await service.UpdateAsync(customerid, options);
                var customer = "";
                if (response == null) return customer;
                customer = response.Id;
                return customer;
            }
            catch (Exception ex)
            {
                Error = true;
                ErrorMessage = ex.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// Obtiene el metodo de pago actual de un cliente
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public async Task<Card> GetCustomerPaymethod(string customerid)
        {
            ResetError();
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);
                Customer customer = await new CustomerService().GetAsync(customerid);
                var cardid = customer.DefaultSourceId;
                var card = await new CardService().GetAsync(customerid, cardid);
                return card;
            }
            catch (Exception ex)
            {
                Error = true;
                ErrorMessage = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// Elimina la forma de pago de un cliente
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCustomerPaymethod(string customerid, string cardid)
        {
            ResetError();
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);
                var card = await new CardService().DeleteAsync(customerid, cardid);
                if (card != null) return card.Deleted ?? false;
            }
            catch (Exception ex)
            {
                Error = true;
                ErrorMessage = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// Crea un cargo a la forma de pago del cliente
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="customerid"></param>
        /// <param name="description"></param>
        /// <param name="email"></param>
        /// <param name="capture"></param>
        /// <returns></returns>
        public async Task<string> CreateCharge(long amount, string currency, string customerid, string description, string email, bool capture = false)
        {
            ResetError();
            try
            {
                email = email.Trim().TrimStart().TrimEnd();
                StripeConfiguration.SetApiKey(SecretKey);
                var options = new ChargeCreateOptions
                {
                    Amount = amount,
                    Currency = currency,
                    CustomerId = customerid,
                    Description = description,
                    Capture = capture,
                    ReceiptEmail = email
                };
                var service = new ChargeService();
                var result = await service.CreateAsync(options);
                if (result != null && result.Paid) return result.Id;
            }
            catch (Exception ex)
            {
                Error = true;
                ErrorMessage = ex.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// Captura un cargo
        /// </summary>
        /// <param name="chargeid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<bool> CaptureCharge(string chargeid, long amount)
        {
            ResetError();
            try
            {
                var charge = await GetChargeAsync(chargeid);

                if (charge == null) return false;

                if (charge.Captured.HasValue && charge.Captured.Value) return true;

                if (amount > charge.Amount)
                {
                    amount = charge.Amount;
                }

                var options = new ChargeCaptureOptions
                {
                    Amount = amount
                };

                StripeConfiguration.SetApiKey(SecretKey);

                var service = new ChargeService();

                var result = await service.CaptureAsync(chargeid, options);

                return result != null && result.Paid && result.Captured.HasValue && result.Captured.Value;
            }
            catch (Exception ex)
            {
                TrackError(ex);
            }

            return false;
        }

        public async Task<Charge> GetChargeAsync(string chargeid)
        {
            ResetError();
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);
                var service = new ChargeService();
                var charge = await service.GetAsync(chargeid);
                return charge;
            }
            catch (Exception ex)
            {
                TrackError(ex);
            }
            return null;
        }

        /// <summary>
        /// Obtiene la lista de cargos hechos al cliente
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Charge>> GetChargeListAsync(string customerid)
        {
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);
                var service = new ChargeService();
                var charges = await service.ListAsync(new ChargeListOptions
                {
                    Limit = 30,
                    CustomerId = customerid
                });
                return charges.Data;
            }
            catch (Exception ex)
            {
                TrackError(ex);
            }
            return new List<Charge>();
        }

        private void ResetError()
        {
            Error = false;
            ErrorMessage = "";
        }

        /// <summary>
        /// Devuelve la lista de pagos con base a un destino
        /// </summary>
        /// <param name="destination">ID Of external account</param>
        /// <returns></returns>
        public async Task<List<Payout>> GetPayoutList(string destination)
        {
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);
                var service = new PayoutService();
                var charges = await service.ListAsync(new PayoutListOptions
                {
                    Limit = 30,
                    Destination = destination
                });
                return charges.Data;
            }
            catch { }
            return new List<Payout>();
        }

        /// <summary>
        /// Cancela un cargo
        /// </summary>
        /// <param name="chargeid"></param>
        /// <returns></returns>
        public async Task<bool> CancelCharge(string chargeid)
        {
            ResetError();
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);

                var charge = await GetChargeAsync(chargeid);
                if (charge == null) return false;

                if (charge.Refunds?.Data != null)
                {
                    if (charge.Refunds.Data.Count > 0) return true;
                }

                var options = new RefundCreateOptions
                {
                    ChargeId = chargeid,
                };
                var service = new RefundService();
                var refund = await service.CreateAsync(options);
                return refund != null && refund.Status == "succeeded";
            }
            catch (Exception ex)
            {
                TrackError(ex);
            }
            return false;
        }

        /// <summary>
        /// Crea un pago
        /// </summary>
        /// <param name="amount">cantidad en centavos</param>
        /// <param name="destination">destino [source]</param>
        /// <param name="description">descrocion del pago</param>
        /// <returns></returns>
        public async Task<Payout> CreatePayout(long amount, string destination, string description)
        {
            ResetError();
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);
                var options = new PayoutCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Destination = destination,
                    Description = description
                };
                var service = new PayoutService();
                return await service.CreateAsync(options);
            }
            catch (Exception ex)
            {
                TrackError(ex);
            }
            return null;
        }

        /// <summary>
        /// Refund charge
        /// </summary>
        /// <param name="chargeid"></param>
        /// <param name="qtytorefund"></param>
        /// <param name="reason">User RefundReasons.Something</param>
        /// <returns></returns>
        public async Task<bool> Refund(string chargeid, long qtytorefund, string reason)
        {
            ResetError();
            try
            {
                StripeConfiguration.SetApiKey(SecretKey);

                var charge = await GetChargeAsync(chargeid);
                if (charge == null) return false;

                if (charge.Refunds?.Data != null)
                {
                    if (charge.Refunds.Data.Count > 0) return true;
                }

                var options = new RefundCreateOptions
                {
                    ChargeId = chargeid,
                    Amount = qtytorefund,
                    Reason = reason
                };
                var service = new RefundService();
                var refund = await service.CreateAsync(options);
                return refund?.Status == "succeeded";
            }
            catch (Exception ex)
            {
                TrackError(ex);
            }
            return false;
        }

        public void TrackError(Exception ex)
        {
            Error = true;
            ErrorMessage = ex.Message;
        }
    }
}
