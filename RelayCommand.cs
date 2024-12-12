using System.Windows.Input;

namespace WpfApp_Concesionario
{
    public class RelayCommand : ICommand
    {
        public Func<Task> _executeFunc;
        public Action _executeAct;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Func<Task> execute)
        {
            this._executeFunc = execute;
        }

        public RelayCommand(Action execute)
        {
            this._executeAct = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public async void Execute(object? parameter)
        {
            if (this._executeFunc != null)
            {
                await this._executeFunc();
            }
            else
            {
                this._executeAct();
            }
        }
    }
}
