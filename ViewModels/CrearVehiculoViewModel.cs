using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WpfApp_Concesionario.Models;
using WpfApp_Concesionario.Services;

namespace WpfApp_Concesionario.ViewModels
{
    public class CrearVehiculoViewModel : INotifyPropertyChanged
    {
        private readonly CocheService _cocheService;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand GuardarCoche { get; private set; }
        public CocheModel Coche { get; set; }

        public CrearVehiculoViewModel(CocheService cocheService)
        {
            _cocheService = cocheService;
            this.GuardarCoche = new RelayCommand(async () => await CrearVehiculoAsync());
            this.Coche = new CocheModel();
        }

        public CrearVehiculoViewModel() { }

        private async Task CrearVehiculoAsync()
        {
            bool estaVacio = string.IsNullOrEmpty(Coche.FirstName) || string.IsNullOrEmpty(Coche.LastName) || string.IsNullOrEmpty(Coche.Country) || string.IsNullOrEmpty(Coche.CarBrand) || string.IsNullOrEmpty(Coche.CarModel) || string.IsNullOrEmpty(Coche.CarColor) || Coche.YearOfManufacture == 0 || string.IsNullOrEmpty(Coche.CreditCardType);
            if (estaVacio) MessageBox.Show("Debes rellenar todos los campos", "Warning");

            await this._cocheService.POSTCocheAsync(Coche);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
