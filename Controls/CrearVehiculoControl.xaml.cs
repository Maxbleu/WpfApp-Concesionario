using System.Windows.Controls;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario.Controls
{
    /// <summary>
    /// Lógica de interacción para CrearVehiculosControl.xaml
    /// </summary>
    public partial class CrearVehiculoControl : UserControl
    {
        public CrearVehiculoControl(CrearVehiculoViewModel crearVehiculoViewModel)
        {
            InitializeComponent();
            this.DataContext = crearVehiculoViewModel;
        }
    }
}
