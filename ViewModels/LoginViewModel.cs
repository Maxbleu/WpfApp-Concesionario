using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WpfApp_Concesionario.Models;
using WpfApp_Concesionario.Services;

namespace WpfApp_Concesionario.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private AuthService _authService;
        private bool _isAuthenticated;
        public event PropertyChangedEventHandler? PropertyChanged;
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

        public LoginViewModel(AuthService authService)
        {
            LoginCommand = new RelayCommand(async () => await LoginAsync());
            this.User = new UserModel();
            this._authService = authService;
        }

        public LoginViewModel() { }

        private async Task LoginAsync()
        {
            bool estaVacio = string.IsNullOrEmpty(User.Username) || string.IsNullOrEmpty(User.Password);
            if(estaVacio) MessageBox.Show("Debes introducir tanto el usuario como la contraseña","Warning");

            bool isAuthenticated = await this._authService.LoginAsync(User);

            this.IsAuthenticated = isAuthenticated;

            OnPropertyChanged(nameof(IsAuthenticated));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
