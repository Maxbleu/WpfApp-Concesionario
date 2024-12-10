using System.Windows.Controls;
using System.Windows.Data;
using WpfApp_Concesionario.Models;
using WpfApp_Concesionario.Services;

namespace WpfApp_Concesionario.Controls
{
    /// <summary>
    /// Lógica de interacción para ListarVehiculosControl.xaml
    /// </summary>
    public partial class ListarVehiculosControl : UserControl
    {
        private readonly CocheService _cocheService;
        public ListarVehiculosControl(CocheService cocheService)
        {
            InitializeComponent();
            this._cocheService = cocheService;
        }

        private async void Window_LoadedAsync(object sender, System.Windows.RoutedEventArgs e)
        {
            List<CocheModel> vehiculos = await this._cocheService.GETCochesAsync();
            CollectionViewSource collectionViewSource = ((CollectionViewSource)(FindResource("vehiculosViewSource")));
            collectionViewSource.Source = vehiculos;
        }
    }
}
