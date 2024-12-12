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
        private readonly ListarVehiculosViewModel _listarVehiculosViewModel;
        private readonly LoginViewModel _loginViewModel;
        private readonly CocheService _cocheService;

        private UserControl _currentContent;
        private bool _isCocheChecked;
        public event PropertyChangedEventHandler? PropertyChanged;

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
        public ICommand NavegarControlListarVehiculosCommand { get; private set; }
        public ICommand NavegarControlCrearVehiculoCommand { get; private set; }
        public ICommand NavegarControlModificarVehiculoCommand { get; private set; }
        public ICommand EliminarVehiculoCommad { get; private set; }

        public MainWindowViewModel()
        {
            this._listarVehiculosViewModel = InstanceServiceProvider.GetService<ListarVehiculosViewModel>();
            this._listarVehiculosViewModel.PropertyChanged += ListarVehiculosViewModel_PropertyChanged;

            this._loginViewModel = InstanceServiceProvider.GetService<LoginViewModel>();
            this._loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
            
            CurrentContent = InstanceServiceProvider.GetService<LoginControl>();

            this.NavegarControlListarVehiculosCommand = new RelayCommand(NavigateToListarVehiculos);
            this.NavegarControlCrearVehiculoCommand = new RelayCommand(NavigateToCrearVehiculo);
            this.NavegarControlModificarVehiculoCommand = new RelayCommand(NavigateToModificarVehiculo);
            this.EliminarVehiculoCommad = new RelayCommand(EliminarVehiculoAsync);

            this._cocheService = InstanceServiceProvider.GetService<CocheService>();
        }

        private void ListarVehiculosViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.IsCocheChecked = true;
        }
        private void LoginViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.IsAuthenticated))
            {
                if (LoginViewModel.IsAuthenticated)
                {
                    CurrentContent = InstanceServiceProvider.GetService<ListarVehiculosControl>();
                }
                else
                {
                    CurrentContent = InstanceServiceProvider.GetService<LoginControl>();
                }
            }
        }

        public void NavigateToListarVehiculos()
        {
            CurrentContent = InstanceServiceProvider.GetService<ListarVehiculosControl>();
            this.IsCocheChecked = false;
        }
        private void NavigateToCrearVehiculo()
        {
            if (this._listarVehiculosViewModel.VehiculoSeleccionado == null)
            {
                CrearVehiculoViewModel crearVehiculoViewModel = CrearVehiculoViewModelFactory.Create(this._cocheService,Enums.ModoManipulacionDatos.CREATE);
                CurrentContent = CrearVehiculoControlFactory.Create(crearVehiculoViewModel);
            }
            else
            {
                MessageBox.Show("No puedes crear un vehículo si tienes uno seleccionado", "Warning");
            }
        }
        private void NavigateToModificarVehiculo()
        {
            if (this._listarVehiculosViewModel.VehiculoSeleccionado != null)
            {
                CrearVehiculoViewModel crearVehiculoViewModel = CrearVehiculoViewModelFactory.Create(this._cocheService, Enums.ModoManipulacionDatos.UPDATE);
                CurrentContent = CrearVehiculoControlFactory.Create(crearVehiculoViewModel);
            }
            else
            {
                MessageBox.Show("Debes seleccionar un vehículo para modificar", "Warning");
            }
        }
        private async Task EliminarVehiculoAsync()
        {
            CocheService cocheService = InstanceServiceProvider.GetService<CocheService>();
            await cocheService.DELETECocheAsync(this._listarVehiculosViewModel.VehiculoSeleccionado.Id);
            this._listarVehiculosViewModel.ListaVehiculos.Remove(this._listarVehiculosViewModel.VehiculoSeleccionado);
            this.IsCocheChecked = false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
