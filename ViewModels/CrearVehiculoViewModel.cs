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
        //  CAMPOS
        private readonly CocheService _cocheService;
        private readonly ModoManipulacionDatos _modoManipulacionDatos;
        private readonly ListarVehiculosViewModel _listarVehiculosViewModel;
        private readonly MainWindowViewModel _mainWindowViewModel;
        public event PropertyChangedEventHandler? PropertyChanged;

        //  PROPIEDADES
        public ICommand GuardarCoche { get; private set; }
        public CocheModel Coche { get; set; }

        public CrearVehiculoViewModel(CocheService cocheService, ModoManipulacionDatos modoManipulacionDatos)
        {
            _cocheService = cocheService;
            this.GuardarCoche = new RelayCommand(async () => await CrearVehiculoAsync());

            //  Obtenemos las vistas necesarias para la navegación
            this._listarVehiculosViewModel = InstanceServiceProvider.GetService<ListarVehiculosViewModel>();
            this._mainWindowViewModel = InstanceServiceProvider.GetService<MainWindowViewModel>();

            //  Comprobamos si estamos en modo de creación o actualización y que no tengamos un vehículo seleccionado
            if (this._listarVehiculosViewModel.VehiculoSeleccionado == null && modoManipulacionDatos == ModoManipulacionDatos.CREATE)
            {
                //  Creamos un nuevo vehículo
                this.Coche = new CocheModel();
            }
            else
            {
                //  Obtenemos el vehículo seleccionado
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

            //  Guardamos el modo de manipulación de datos
            this._modoManipulacionDatos = modoManipulacionDatos;
        }

        /// <summary>
        /// 
        /// CREA un vehículo y envia una solicitud a la base de datos para
        /// crearlo y una vez obtenida la respuesta, lo añade a la lista de vehículos.
        /// 
        /// En caso de ser ACTUALIZACIÓN, se obtienen los registros actualiazados y 
        /// junto con el id del vehículo y las propiedades modificadas con sus valores
        /// son enviadas a la base de datos para su actualización y posteriormente
        /// se obtiene el vehículo actualizado y se actualiza en la lista de vehículos.
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task CrearVehiculoAsync()
        {
            //  Comprobamos que los campos no estén vacíos
            if (string.IsNullOrEmpty(Coche.FirstName) || string.IsNullOrEmpty(Coche.LastName) || string.IsNullOrEmpty(Coche.Country) || string.IsNullOrEmpty(Coche.CarBrand) || string.IsNullOrEmpty(Coche.CarModel) || string.IsNullOrEmpty(Coche.CarColor) || Coche.YearOfManufacture == 0 || string.IsNullOrEmpty(Coche.CreditCardType))
            {
                //  Mostramos un mensaje de advertencia
                MessageBox.Show("Debes rellenar todos los campos", "Warning");
            }
            else
            {
                //  Comprobamos si estamos en modo de creación o actualización
                if (this._modoManipulacionDatos == ModoManipulacionDatos.CREATE)
                {
                    //  Enviamos la solicitud a la base de datos para crear el coche
                    CocheModel cocheCreado = await this._cocheService.POSTCocheAsync(Coche);

                    //  Añadimos el coche a la lista de vehículos
                    this._listarVehiculosViewModel.ListaVehiculos.Add(cocheCreado);
                }
                else
                {
                    //  Enviamos la solicitud a la base de datos para actualizar el coche
                    await ActualizarVehiculoAsync();
                }

                //  Navegamos a la lista de vehículos
                this._mainWindowViewModel.NavigateToListarVehiculos();

                //  Mostramos un mensaje de información
                MessageBox.Show("Coche guardado correctamente", "Info");
            }
        }

        /// <summary>
        /// En caso de que la acción sea de actualización se optienen los campos
        /// actualizados con sus respectivos valores y se envía una solicitud a la
        /// base de datos para actualizar el vehículo. Una vez obtenida la respuesta
        /// actualizamos el coche en la lista de vehículos.
        /// </summary>
        /// <returns></returns>
        private async Task ActualizarVehiculoAsync()
        {
            Dictionary<string, JsonElement> propiedadesModificadas = new Dictionary<string, JsonElement>();

            //  Obtenemos los campos que se han actualizado
            Type type = typeof(CocheModel);
            PropertyInfo[] propiedades = type.GetProperties();

            //  Recorremos las propiedades del objeto
            foreach (PropertyInfo propiedad in propiedades)
            {
                string nombrePropiedad = propiedad.Name;

                //  Obtenemos el valor de la propiedad modificada y el valor de la propiedad original
                object valorPropiedadObjModificada = propiedad.GetValue(this.Coche);
                object valorPropiedadObjOriginal = propiedad.GetValue(this._listarVehiculosViewModel.VehiculoSeleccionado);

                //  Comprobamos si el valor de la propiedad modificada es diferente al valor de la propiedad original
                if (!valorPropiedadObjModificada.Equals(valorPropiedadObjOriginal))
                {
                    //  Comprobamos el tipo de la propiedad para parsear el valor
                    if (propiedad.PropertyType == typeof(int))
                    {
                        //  Parseamos el valor de la propiedad a un entero
                        int valorPropiedadInt = (int)valorPropiedadObjModificada;

                        //  Añadimos la propiedad modificada al diccionario
                        propiedadesModificadas.Add(nombrePropiedad, JsonDocument.Parse(valorPropiedadInt.ToString()).RootElement);
                    }
                    else if (propiedad.PropertyType == typeof(string))
                    {
                        //  Parseamos el valor de la propiedad a un string
                        string valorPropiedadString = (string)valorPropiedadObjModificada;

                        //  Añadimos la propiedad modificada al diccionario
                        propiedadesModificadas.Add(nombrePropiedad, JsonDocument.Parse($"\"{valorPropiedadString}\"").RootElement);
                    }
                }
            }

            //  Enviamos la solicitud a la base de datos para actualizar el coche
            CocheModel cocheModificado = await this._cocheService.UPDATECocheAsync(this._listarVehiculosViewModel.VehiculoSeleccionado.Id, propiedadesModificadas);

            //  Actualizamos el coche en la lista de vehículos
            this._listarVehiculosViewModel.ListaVehiculos[this._listarVehiculosViewModel.VehiculoSeleccionado.Id - 1] = Coche;
        }
        /// <summary>
        /// Notifica a la vista que una propiedad ha cambiado.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
