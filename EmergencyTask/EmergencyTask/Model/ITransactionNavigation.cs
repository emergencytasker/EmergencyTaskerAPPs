using System.Threading.Tasks;

namespace EmergencyTask.Model
{
    public interface ITransactionNavigation
    {

        Task Execute(TransactionModel model);

    }
}
