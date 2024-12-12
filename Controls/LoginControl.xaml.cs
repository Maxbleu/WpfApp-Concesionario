using System.Windows.Controls;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario.Controls
{
    /// <summary>
    /// Lógica de interacción para LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControl(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            this.DataContext = loginViewModel;
        }
    }
}
