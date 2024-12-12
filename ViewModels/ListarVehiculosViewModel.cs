using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfApp_Concesionario.DI;
using WpfApp_Concesionario.Models;
using WpfApp_Concesionario.Services;

namespace WpfApp_Concesionario.ViewModels
{
    public class ListarVehiculosViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CocheModel> _listaVehiculos;
        private CocheModel _vehiculoSeleccionado;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<CocheModel> ListaVehiculos
        {
            get => _listaVehiculos;
            set
            {
                if (_listaVehiculos != value)
                {
                    _listaVehiculos = value;
                    OnPropertyChanged(nameof(ListaVehiculos));
                }
            }
        }
        public CocheModel VehiculoSeleccionado
        {
            get => _vehiculoSeleccionado;
            set
            {
                if (_vehiculoSeleccionado != value)
                {
                    _vehiculoSeleccionado = value;
                    OnPropertyChanged(nameof(VehiculoSeleccionado));
                }
            }
        }

        public ListarVehiculosViewModel()
        {
            CargarVehiculosAsync();
        }

        private async void CargarVehiculosAsync()
        {
            var cocheService = InstanceServiceProvider.GetService<CocheService>();
            List<CocheModel> vehiculos = await cocheService.GETCochesAsync();
            this._listaVehiculos = new ObservableCollection<CocheModel>(vehiculos);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    
}
