using System.Windows.Input;

namespace WpfApp_Concesionario
{
    /// <summary>
    /// Esta clase nos facilita la creación de comandos
    /// para las acciones en el patron MVVM
    /// </summary>
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

        /// <summary>
        /// Este método se encarga de permitir que
        /// ejecuten los comandos.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            return true;
        }
        /// <summary>
        /// Este método se encarga de ejecutar los comandos
        /// dependiendo de si es un comando asíncrono o no.
        /// </summary>
        /// <param name="parameter"></param>
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
