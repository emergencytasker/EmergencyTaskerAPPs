using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.ViewModel.Business;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Extensions;
using EmergencyTask.ViewModel.Validators;
using Stripe;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class PaymentHistoryViewModel : ViewModelBase
    {

        #region Notified Property Transactions
        /// <summary>
        /// Transactions
        /// </summary>
        private IList<TransactionModel> transactions;
        public IList<TransactionModel> Transactions
        {
            get { return transactions; }
            set { transactions = value; OnPropertyChanged(); }
        }
        #endregion

        public IEnumerable<Hito> Hitos { get; set; }

        public PaymentHistoryViewModel()
        {
            
        }

        private async void GoToTransaction_Command(object obj, IExecuteValidator[] args)
        {
            if (!(obj is TransactionModel model)) return;
            var factory = new EnumFactory<TransactionType, ITransactionNavigation>("EmergencyTask.ViewModel.Business.Payments.", "Navigation");
            var navigation = factory.Resolve(model.TransactionType);
            if (navigation == null) return;
            await navigation.Execute(model);
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            
            IsBusy = true;

            var hitos = await GetHitos();
            var balancehistory = await GetBalanceHistory();

            var transactions = new List<TransactionModel>();
            transactions.AddRange(hitos);
            transactions.AddRange(balancehistory);
            
            SetSource(transactions);

            IsBusy = false;
        }

        private void SetSource(List<TransactionModel> transactions)
        {
            Transactions = transactions.OrderByDescending(t => t.Time).ToList();
        }

        private async Task<IEnumerable<TransactionModel>> GetStripeHistory()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return new List<TransactionModel>(0);
            var stripe = await App.GetStripeAsync();
            var customerid = await usuario.GetCustomerId();
            return ((await stripe?.GetChargeListAsync(customerid)) ?? new List<Charge>()).Select(charge => new TransactionModel(charge));
        }

        private async Task<IEnumerable<TransactionModel>> GetHitos()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return new List<TransactionModel>(0);
            Hitos = await Client.Hito.Where(new Hito
            {
                trabajador = usuario.Perfil == API.Enum.Perfil.Tasker ? usuario.id : 0,
                cliente = usuario.Perfil == API.Enum.Perfil.Client ? usuario.id : 0
            }) ?? new List<Hito>();
            var transactions = new List<TransactionModel>(Hitos.Count());
            foreach (var item in Hitos.Where(h => h.estado > (int) HitoStatus.Created))
            {
                transactions.Add(new TransactionModel(item)
                {
                    GoToTransaction = new ExtendCommand(GoToTransaction_Command, new InternetValidator(), new UserValidator())
                });
            }
            return transactions;
        }

        private async Task<IEnumerable<TransactionModel>> GetBalanceHistory()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return new List<TransactionModel>(0);
            var items = await Client.Balance.Where(new API.ER.Balance
            {
                idusuario = usuario.id,
                idperfil = usuario.idperfil
            });
            var filter = items.Where(i => !Hitos.Any(h => h.id == (i.idhito ?? -1)));
            var transactions = new List<TransactionModel>(filter.Count());
            foreach (var item in filter)
            {
                transactions.Add(new TransactionModel(item)
                {
                    GoToTransaction = new ExtendCommand(GoToTransaction_Command, new InternetValidator(), new UserValidator())
                });
            }
            return transactions;
        }

    }
}