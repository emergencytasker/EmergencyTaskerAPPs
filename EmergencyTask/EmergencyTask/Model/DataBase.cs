using Plugin.Storage.Documental;
using System.Linq;

namespace EmergencyTask.Model
{
    public class DataBase
    {

        public DataBase()
        {
            Usuario = YunoConnection.Instance.Query<Usuario>();
            Variables = YunoConnection.Instance.Query<Variable>();
        }

        public DocumentQuery<Usuario> Usuario { get; }
        private DocumentQuery<Variable> Variables { get; }

        public static Variable GetVariable()
        {
            DataBase db = new DataBase();
            var variable = db.Variables.FirstOrDefault();
            if (variable == null) return new Variable();
            return new Variable
            {
                Introduction = variable.Introduction,
                AppCenterId = variable.AppCenterId,
                StartWaitConfirmation = variable.StartWaitConfirmation,
                CanReceivedNotifications = variable.CanReceivedNotifications,
                LastNumbers = variable.LastNumbers
            };
        }

        public static async void SetVariable(Variable newvariable)
        {
            DataBase db = new DataBase();
            var variable = db.Variables.FirstOrDefault();
            if (variable == null)
            {
                db.Variables.Add(newvariable);
            }
            else
            {
                db.Variables.Clear();
                db.Variables.Add(new Variable
                {
                    Introduction = variable.Introduction == newvariable.Introduction ? variable.Introduction : newvariable.Introduction,
                    AppCenterId = variable.AppCenterId == newvariable.AppCenterId ? variable.AppCenterId : newvariable.AppCenterId,
                    CanReceivedNotifications = variable.CanReceivedNotifications == newvariable.CanReceivedNotifications ? variable.CanReceivedNotifications : newvariable.CanReceivedNotifications,
                    LastNumbers = newvariable.LastNumbers,
                    StartWaitConfirmation = variable.StartWaitConfirmation == newvariable.StartWaitConfirmation ? variable.StartWaitConfirmation : newvariable.StartWaitConfirmation
                });
            }
            await db.Variables.SaveChanges();
        }
    }
}
