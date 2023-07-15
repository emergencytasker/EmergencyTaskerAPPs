using System;
using Acr.UserDialogs;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;

namespace EmergencyTask.ViewModel.Commands
{
    public class ExtendCommand : ICommand
    {
        /// <summary>
        /// Almacena las instancias de las posibles validaciones del command
        /// </summary>
        private IExecuteValidator[] ICanExecute { get; }

        /// <summary>
        /// Action para el command
        /// </summary>
        private Action<object, IExecuteValidator[]> Action { get; }

        public event EventHandler CanExecuteChanged;

        public ExtendCommand(Action<object, IExecuteValidator[]> execute, params IExecuteValidator[] icanexecute)
        {
            Action = execute;
            ICanExecute = icanexecute;
        }

        public ExtendCommand(Action<object, IExecuteValidator[]> execute, params Func<bool>[] icanexecute)
        {
            Action = execute;
            if (icanexecute != null)
            {
                ICanExecute = new IExecuteValidator[icanexecute.Length];
                for (int i = 0; i < icanexecute.Length; i++)
                {
                    ICanExecute[i] = new ExecuteValidator(string.Empty, icanexecute[i]);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        private bool GetCanExecute()
        {
            if (ICanExecute == null) return true;
            foreach (var canexecute in ICanExecute)
            {
                if (canexecute.Rule()) continue;
                if (!string.IsNullOrEmpty(canexecute.ErrorMessage))
                {
                    UserDialogs.Instance.Toast(canexecute.ErrorMessage);
                }
                return false;
            }
            return true;
        }

        public void Execute(object parameter)
        {
            if (!GetCanExecute()) return;
            Action?.Invoke(parameter, ICanExecute);
        }
    }

    public static class ValidatorExtensions
    {
        public static bool TryGetComparator<T>(this IExecuteValidator[] validators, out T type)
        {
            var found = validators.FirstOrDefault(v => v.Comparator is T);
            if (found == null)
            {
                type = default(T);
                return false;
            }
            try
            {
                type = (T)Convert.ChangeType(found.Comparator, typeof(T));
                return true;
            }
            catch
            {
                type = default(T);
            }
            return false;
        }
    }

    public interface IExtendCommandEvents
    {
        void OnPreExecute();
        void OnPostExecute();
    }

    public interface IExecuteValidator
    {
        string ErrorMessage { get; }
        bool Rule();
        object Comparator { get; }
        void Execute();
    }

    public class ExecuteValidator : IExecuteValidator
    {
        public string ErrorMessage { get; }
        public object Comparator { get; }
        private Func<bool> CanExecute { get; }

        public ExecuteValidator(string errormessage, Func<bool> canexecute)
        {
            ErrorMessage = errormessage;
            CanExecute = canexecute;
        }

        public void Execute() { }

        public bool Rule() => CanExecute?.Invoke() ?? false;
    }

}