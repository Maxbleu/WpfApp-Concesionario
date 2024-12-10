using System.Windows.Controls;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario.Controls
{
    /// <summary>
    /// Lógica de interacción para CrearVehiculosControl.xaml
    /// </summary>
    public partial class CrearVehiculoControl : UserControl
    {
        private readonly CrearVehiculoViewModel _crearVehiculoViewModel;
        public CrearVehiculoControl(CrearVehiculoViewModel crearVehiculoViewModel)
        {
            InitializeComponent();
            this._crearVehiculoViewModel = crearVehiculoViewModel;
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.controladorCrearVehiculo.DataContext = this._crearVehiculoViewModel;
        }
    }
}
