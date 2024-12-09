using System.Windows;
using System.Windows.Controls;
using WpfApp_Concesionario.Controls;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ListarVehiculosControl _listarVehiculosControl;
        private readonly LoginControl _loginControl;
        private readonly LoginViewModel _loginViewModel;
        private readonly CrearVehiculoControl _crearVehiculoControl;
        private readonly ModificarVehiculoControl _modificarVehiculoControl;
        private readonly EliminarVehiculoControl _eliminarVehiculoControl;
        public MainWindow(ListarVehiculosControl listarVehiculosControl, LoginControl loginControl, LoginViewModel loginViewModel, CrearVehiculoControl crearVehiculoControl, ModificarVehiculoControl modificarVehiculoControl, EliminarVehiculoControl eliminarVehiculoControl)
        {
            InitializeComponent();
            this._listarVehiculosControl = listarVehiculosControl;
            this._loginControl = loginControl;
            this._loginViewModel = loginViewModel;
            this._crearVehiculoControl = crearVehiculoControl;
            this._modificarVehiculoControl = modificarVehiculoControl;
            this._eliminarVehiculoControl = eliminarVehiculoControl;

            this.DataContext = this._loginViewModel;

            ((LoginViewModel)DataContext).PropertyChanged += MainWindow_PropertyChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainContent.Content = this._loginControl;
        }
        private void RefreshContent(UserControl control)
        {
            MainContent.Content = null;
            MainContent.Content = control;
        }
        private void MainWindow_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this._loginViewModel.IsAuthenticated))
            {
                if (((LoginViewModel)DataContext).IsAuthenticated)
                {
                    RefreshContent(_listarVehiculosControl);
                }
                else
                {
                    RefreshContent(_loginControl);
                }
            }
        }

        private void Menu_Click_OpenListarVehiculos(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = this._listarVehiculosControl;
        }
        private void Menu_Click_OpenCrearVehiculo(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = this._crearVehiculoControl;
        }
        private void Menu_Click_OpenModificarVehiculo(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = this._modificarVehiculoControl;
        }
        private void Menu_Click_OpenEliminarVehiculo(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = this._eliminarVehiculoControl;
        }

    }
}     