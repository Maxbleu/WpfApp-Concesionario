using System.Windows.Input;

namespace WpfApp_Concesionario
{
    public class RelayCommand : ICommand
    {
        public Func<Task> _execute;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Func<Task> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            if (this._execute != null)
            {
                await this._execute();
            }
        }
    }
}
