using System.Windows.Controls;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario.Controls
{
    /// <summary>
    /// Lógica de interacción para ListarVehiculosControl.xaml
    /// </summary>
    public partial class ListarVehiculosControl : UserControl
    {
        public ListarVehiculosControl(ListarVehiculosViewModel listarVehiculosViewModel)
        {
            InitializeComponent();
            this.DataContext = listarVehiculosViewModel;
        }
    }
}
