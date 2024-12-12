using System;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WpfApp_Concesionario.Controls;
using WpfApp_Concesionario.DI;
using WpfApp_Concesionario.Services;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            InstanceServiceProvider.SetServiceProvider(_serviceProvider);
        }
        private void ConfigureServices(ServiceCollection services)
        {

            //  Cargamos el fichero de configuracion
            var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            //  SERVICIOS
            services.AddSingleton<CocheService>();
            services.AddSingleton<AuthService>();

            //  LOGIN
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<LoginControl>();

            //  LISTAR VEHICULOS
            services.AddSingleton<ListarVehiculosViewModel>();
            services.AddSingleton<ListarVehiculosControl>();

            //  CREAR VEHICULOS
            services.AddTransient<CrearVehiculoViewModel>();
            services.AddTransient<CrearVehiculoControl>();

            //  ELIMINAR VEHICULO
            services.AddSingleton<EliminarVehiculoControl>();

            //  MAIN WINDOW
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }

}
