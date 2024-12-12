namespace WpfApp_Concesionario.DI
{
    /// <summary>
    /// Clase que permite obtener servicios en cualquier parte de la aplicacion
    /// a partir del contenedor de dependencias que inicializamos en App.xaml.cs
    /// en el inicio de la aplicación
    /// </summary>
    public class InstanceServiceProvider
    {
        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// Inicializamos el contenedor de dependencias
        /// que se ha creado en App.xaml.cs
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Obtenemos un servicio a partir del contenedor
        /// de dependencias que se ha inicializado en App.xaml.cs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }
    }
}
