using System.Windows;
using System.Windows.Controls;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario.Controls
{
    /// <summary>
    /// Lógica de interacción para LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private readonly LoginViewModel _loginViewModel;
        public LoginControl(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            this._loginViewModel = loginViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.controladorIniciarSesion.DataContext = this._loginViewModel;
        }
    }
}
