using Acr.UserDialogs;
using System;
using System.Windows.Input;

namespace EmergencyTask.Model
{
    public class UserCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<object, Usuario> Command { get; set; }

        public UserCommand(Action<object, Usuario> command)
        {
            Command = command;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        private bool HasBeenRaised = false;
        public async void Execute(object parameter)
        {
            if (HasBeenRaised) return;
            HasBeenRaised = true;
            var usuario = Usuario.GetUserLogin();
            if(usuario == null)
            {
                await UserDialogs.Instance.AlertAsync("Tu sesion ha caducado", "Info", "Aceptar");
                var app = App.Current as App;
                app.Restart();
            }
            else
            {
                Command?.Invoke(parameter, usuario);
            }
            HasBeenRaised = false;
        }
    }
}
