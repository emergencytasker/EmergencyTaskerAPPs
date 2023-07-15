using EmergencyTask.Model;
using EmergencyTask.ViewModel.Commands;
using System;

namespace EmergencyTask.ViewModel.Validators
{
    public class UserValidator : IExecuteValidator
    {
        public string ErrorMessage => "Tu sesion ha expirado";
        public object Comparator { get; private set;  }

        public void Execute()
        {
            if (!Rule())
            {
                Usuario.Delete();
                (Xamarin.Forms.Application.Current as App).Restart();
                throw new Exception(ErrorMessage);
            }
        }

        public bool Rule()
        {
            Comparator = Usuario.GetUserLogin();
            return Comparator != null;
        }
    }
}