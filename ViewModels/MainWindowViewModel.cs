using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp_Concesionario.Controls;
using WpfApp_Concesionario.DI;
using WpfApp_Concesionario.Factory;
using WpfApp_Concesionario.Services;

namespace WpfApp_Concesionario.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //  CAMPOS
        private readonly ListarVehiculosViewModel _listarVehiculosViewModel;
        private readonly CocheService _cocheService;

        private readonly LoginViewModel _loginViewModel;
        private UserControl _currentContent;
        private bool _isCocheChecked;
        private string _menuVisibility = "Hidden";

        public event PropertyChangedEventHandler? PropertyChanged;

        //  PROPIEDADES
        public LoginViewModel LoginViewModel
        {
            get
            {
                return this._loginViewModel;
            }
        }
        public UserControl CurrentContent
        {
            get => _currentContent;
            set
            {
                _currentContent = value;
                OnPropertyChanged(nameof(CurrentContent));
            }
        }
        public bool IsCocheChecked
        {
            get => _isCocheChecked;
            set
            {
                _isCocheChecked = value;
                OnPropertyChanged(nameof(IsCocheChecked));
            }
        }
        public string MenuVisibility
        {
            get => _menuVisibility;
            set
            {
                _menuVisibility = value;
                OnPropertyChanged(nameof(MenuVisibility));
            }
        }

        //  COMANDOS
        public ICommand NavegarControlListarVehiculosCommand { get; private set; }
        public ICommand NavegarControlCrearVehiculoCommand { get; private set; }
        public ICommand NavegarControlModificarVehiculoCommand { get; private set; }
        public ICommand EliminarVehiculoCommad { get; private set; }

        public MainWindowViewModel()
        {
            //  Inicializamos los servicios, los viewmodels y las subscripciones a los eventos
            this._listarVehiculosViewModel = InstanceServiceProvider.GetService<ListarVehiculosViewModel>();
            this._listarVehiculosViewModel.PropertyChanged += ListarVehiculosViewModel_PropertyChanged;

            this._loginViewModel = InstanceServiceProvider.GetService<LoginViewModel>();
            this._loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;

            //  Inicializamos el control de login
            CurrentContent = InstanceServiceProvider.GetService<LoginControl>();

            //  Inicializamos los comandos
            this.NavegarControlListarVehiculosCommand = new RelayCommand(NavigateToListarVehiculos);
            this.NavegarControlCrearVehiculoCommand = new RelayCommand(NavigateToCrearVehiculo);
            this.NavegarControlModificarVehiculoCommand = new RelayCommand(NavigateToModificarVehiculo);
            this.EliminarVehiculoCommad = new RelayCommand(EliminarVehiculoAsync);

            //  Inicializamos el servicio de coches
            this._cocheService = InstanceServiceProvider.GetService<CocheService>();
        }

        //  EVENTOS SUBCRITOS

        /// <summary>
        /// Evento que se dispara cuando se selecciona un vehículo 
        /// en el ListarVehiculosViewModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListarVehiculosViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.IsCocheChecked = true;
        }
        /// <summary>
        /// Evento que se dispara cuando se autentica un usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.IsAuthenticated))
            {
                //  Comprobamos si el usuario se ha autenticado
                if (LoginViewModel.IsAuthenticated)
                {
                    //  Si se ha autenticado, mostramos el menú de la aplicación
                    this.MenuVisibility = "Visible";

                    //  Navegamos al control de listar vehículos
                    CurrentContent = InstanceServiceProvider.GetService<ListarVehiculosControl>();
                }
                else
                {

                    //  Si no se ha autenticado, mostramos el control de login
                    CurrentContent = InstanceServiceProvider.GetService<LoginControl>();
                }
            }
        }

        //  MÉTODOS DE NAVEGACIÓN
        /// <summary>
        /// Navega al control de listar vehículos
        /// </summary>
        public void NavigateToListarVehiculos()
        {
            //  Si el control actual es el de listar vehículos, no hacemos nada
            CurrentContent = InstanceServiceProvider.GetService<ListarVehiculosControl>();

            //  Reseteamos la propiedad IsCocheChecked
            this.IsCocheChecked = false;
        }
        /// <summary>
        /// Navega al controlador de crear vehículo si el 
        /// usuario no tiene un vehículo seleccionado
        /// </summary>
        private void NavigateToCrearVehiculo()
        {
            //  Comprueba si hay un vehículo seleccionado
            if (this._listarVehiculosViewModel.VehiculoSeleccionado == null)
            {
                //  Si no hay un vehículo seleccionado, creamos un nuevo vehículo
                CrearVehiculoViewModel crearVehiculoViewModel = CrearVehiculoViewModelFactory.Create(this._cocheService,Enums.ModoManipulacionDatos.CREATE);

                //  Mostramos el control de crear vehículos
                CurrentContent = CrearVehiculoControlFactory.Create(crearVehiculoViewModel);
            }
            else
            {
                //  Si hay un vehículo seleccionado, mostramos un mensaje de advertencia
                MessageBox.Show("No puedes crear un vehículo si tienes uno seleccionado", "Warning");
            }
        }
        /// <summary>
        /// Navega al control de modificar vehículo si el 
        /// usuario tiene seleccinado un vehículo
        /// </summary>
        private void NavigateToModificarVehiculo()
        {
            //  Comprobamos si hay un vehículo seleccionado
            if (this._listarVehiculosViewModel.VehiculoSeleccionado != null)
            {
                //  Si hay un vehículo seleccionado, creamos un nuevo vehículo
                CrearVehiculoViewModel crearVehiculoViewModel = CrearVehiculoViewModelFactory.Create(this._cocheService, Enums.ModoManipulacionDatos.UPDATE);

                //  Mostramos el control de crear vehículos
                CurrentContent = CrearVehiculoControlFactory.Create(crearVehiculoViewModel);
            }
            else
            {
                //  Si no hay un vehículo seleccionado, mostramos un mensaje de advertencia
                MessageBox.Show("Debes seleccionar un vehículo para modificar", "Warning");
            }
        }
        /// <summary>
        /// Elimina un vehículo de la base de datos y de la lista de vehículos
        /// </summary>
        /// <returns></returns>
        private async Task EliminarVehiculoAsync()
        {
            //  Eliminamos el vehículo de la base de datos
            await this._cocheService.DELETECocheAsync(this._listarVehiculosViewModel.VehiculoSeleccionado.Id);

            //  Eliminamos el vehículo de la lista de vehículos
            this._listarVehiculosViewModel.ListaVehiculos.Remove(this._listarVehiculosViewModel.VehiculoSeleccionado);

            //  Reseteamos la propiedad IsCocheChecked
            this.IsCocheChecked = false;
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
