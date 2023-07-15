using EmergencyTask.API;
using EmergencyTask.Model;
using System.Threading.Tasks;

namespace EmergencyTask.ViewModel.Business.Payments
{
    public class HitoNavigation : ITransactionNavigation
    {
        public async Task Execute(TransactionModel model)
        {
            if (!model.IdHito.HasValue) return;
            var hito = await Client.Hito.Get(model.IdHito.Value);
            if (hito == null) return;
            var idsolicitudservicio = hito.idsolicitudservicio;
            if(Xamarin.Forms.Application.Current.MainPage.BindingContext is ViewModelBase viewmodel)
            {
                await viewmodel.GoToServiceInfoPage(idsolicitudservicio);
            }
        }
    }
}
