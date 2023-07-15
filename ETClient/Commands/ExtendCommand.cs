using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Windows.Input;

namespace ETClient.Commands
{
    public class ExtendCommand : ICommand
    {
        [Inject] IToastService? toastService { set; get; }

        /// <summary>
        /// Action para el command
        /// </summary>
        private Action<object> Action { get; }

        public event EventHandler CanExecuteChanged;
        public ExtendCommand(Action<object> execute)
        {
            Action = execute;
        }

        public bool CanExecute(object parameter =null)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute()) return;
            Action?.Invoke(parameter);
        }
    }
}
