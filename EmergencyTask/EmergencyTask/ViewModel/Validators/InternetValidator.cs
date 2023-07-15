using EmergencyTask.ViewModel.Commands;
using System;
using Xamarin.Essentials;

namespace EmergencyTask.ViewModel.Validators
{
    public class InternetValidator : IExecuteValidator
    {
        public string ErrorMessage => "Necesitas una conexion a internet";
        public object Comparator => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public void Execute()
        {
            if (!Rule()) throw new Exception(ErrorMessage);
        }

        public bool Rule()
        {
            return (bool) Comparator;
        }
    }
}
