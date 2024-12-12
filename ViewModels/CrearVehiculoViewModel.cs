using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using WpfApp_Concesionario.DI;
using WpfApp_Concesionario.Enums;
using WpfApp_Concesionario.Models;
using WpfApp_Concesionario.Services;

namespace WpfApp_Concesionario.ViewModels
{
    public class CrearVehiculoViewModel : INotifyPropertyChanged
    {
        private readonly CocheService _cocheService;
        private readonly ModoManipulacionDatos _modoManipulacionDatos;
        private readonly ListarVehiculosViewModel _listarVehiculosViewModel;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand GuardarCoche { get; private set; }
        public CocheModel Coche { get; set; }

        public CrearVehiculoViewModel(CocheService cocheService, ModoManipulacionDatos modoManipulacionDatos)
        {
            _cocheService = cocheService;
            this.GuardarCoche = new RelayCommand(async () => await CrearVehiculoAsync());

            this._listarVehiculosViewModel = InstanceServiceProvider.GetService<ListarVehiculosViewModel>();
            this._mainWindowViewModel = InstanceServiceProvider.GetService<MainWindowViewModel>();

            if (this._listarVehiculosViewModel.VehiculoSeleccionado == null && modoManipulacionDatos == ModoManipulacionDatos.CREATE)
            {
                this.Coche = new CocheModel();
            }
            else
            {
                this.Coche = new CocheModel()
                {
                    Id = this._listarVehiculosViewModel.VehiculoSeleccionado.Id,
                    FirstName = this._listarVehiculosViewModel.VehiculoSeleccionado.FirstName,
                    LastName = this._listarVehiculosViewModel.VehiculoSeleccionado.LastName,
                    Country = this._listarVehiculosViewModel.VehiculoSeleccionado.Country,
                    CarBrand = this._listarVehiculosViewModel.VehiculoSeleccionado.CarBrand,
                    CarModel = this._listarVehiculosViewModel.VehiculoSeleccionado.CarModel,
                    CarColor = this._listarVehiculosViewModel.VehiculoSeleccionado.CarColor,
                    YearOfManufacture = this._listarVehiculosViewModel.VehiculoSeleccionado.YearOfManufacture,
                    CreditCardType = this._listarVehiculosViewModel.VehiculoSeleccionado.CreditCardType
                };
            }

            this._modoManipulacionDatos = modoManipulacionDatos;
        }

        private async Task CrearVehiculoAsync()
        {

            if (string.IsNullOrEmpty(Coche.FirstName) || string.IsNullOrEmpty(Coche.LastName) || string.IsNullOrEmpty(Coche.Country) || string.IsNullOrEmpty(Coche.CarBrand) || string.IsNullOrEmpty(Coche.CarModel) || string.IsNullOrEmpty(Coche.CarColor) || Coche.YearOfManufacture == 0 || string.IsNullOrEmpty(Coche.CreditCardType))
            {
                MessageBox.Show("Debes rellenar todos los campos", "Warning");
            }
            else
            {
                if (this._modoManipulacionDatos == ModoManipulacionDatos.CREATE)
                {
                    CocheModel cocheCreado = await this._cocheService.POSTCocheAsync(Coche);
                    this._listarVehiculosViewModel.ListaVehiculos.Add(cocheCreado);
                }
                else
                {
                    Dictionary<string, JsonElement> propiedadesModificadas = new Dictionary<string, JsonElement>();

                    //  Obtenemos los campos que se han actualizado
                    Type type = typeof(CocheModel);
                    PropertyInfo[] propiedades = type.GetProperties();

                    foreach (PropertyInfo propiedad in propiedades)
                    {
                        string nombrePropiedad = propiedad.Name;

                        object valorPropiedadObjModificada = propiedad.GetValue(this.Coche);
                        object valorPropiedadObjOriginal = propiedad.GetValue(this._listarVehiculosViewModel.VehiculoSeleccionado);

                        if (!valorPropiedadObjModificada.Equals(valorPropiedadObjOriginal))
                        {
                            if (propiedad.PropertyType == typeof(int))
                            {
                                int valorPropiedadInt = (int)valorPropiedadObjModificada;
                                propiedadesModificadas.Add(nombrePropiedad, JsonDocument.Parse(valorPropiedadInt.ToString()).RootElement);
                            }
                            else if (propiedad.PropertyType == typeof(string))
                            {
                                string valorPropiedadString = (string)valorPropiedadObjModificada;
                                propiedadesModificadas.Add(nombrePropiedad, JsonDocument.Parse($"\"{valorPropiedadString}\"").RootElement);
                            }
                        }
                    }

                    CocheModel cocheModificado = await this._cocheService.UPDATECocheAsync(this._listarVehiculosViewModel.VehiculoSeleccionado.Id, propiedadesModificadas);
                    this._listarVehiculosViewModel.ListaVehiculos[this._listarVehiculosViewModel.VehiculoSeleccionado.Id - 1] = Coche;
                }

                this._mainWindowViewModel.NavigateToListarVehiculos();

                MessageBox.Show("Coche guardado correctamente", "Info");
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
