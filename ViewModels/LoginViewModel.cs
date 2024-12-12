using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WpfApp_Concesionario.DI;
using WpfApp_Concesionario.Models;
using WpfApp_Concesionario.Services;

namespace WpfApp_Concesionario.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        // CAMPOS
        private bool _isAuthenticated;
        public event PropertyChangedEventHandler? PropertyChanged;

        // PROPIEDADES
        public UserModel User { get; set; }
        public ICommand LoginCommand { get; private set; }
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(async () => await LoginAsync());
            this.User = new UserModel();
        }

        /// <summary>
        /// Metodo que se encarga de realizar la autenticacion del usuario
        /// </summary>
        /// <returns></returns>
        private async Task LoginAsync()
        {
            // Comprobamos que los campos no esten vacios
            bool estaVacio = string.IsNullOrEmpty(User.Username) || string.IsNullOrEmpty(User.Password);
            if(estaVacio) MessageBox.Show("Debes introducir tanto el usuario como la contraseña","Warning");

            // Obtenemos el servicio de autenticacion
            AuthService authService = InstanceServiceProvider.GetService<AuthService>();

            // Realizamos la autenticacion del usuario
            bool isAuthenticated = await authService.LoginAsync(User);
            this.IsAuthenticated = isAuthenticated;

            //  Notificamos a la vista que la propiedad ha cambiado
            OnPropertyChanged(nameof(IsAuthenticated));
        }
        /// <summary>
        /// Notifica a la vista que una propiedad ha cambiado.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
