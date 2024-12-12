using WpfApp_Concesionario.Controls;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario.Factory
{
    /// <summary>
    /// Aplicamos el patron Factory para crear el controlador de
    /// CrearVehiculo ya que, require de un ViewModel para su construccion
    /// y a su vez, el ViewModel requiere de un servicio y un modo
    /// de uso que solo sabremos en tiempo de ejecución por lo que
    /// este patron nos permite encapsular la creación de este controlador.
    /// 
    /// Adaptandose este patron a las necesidades de la aplicacion.
    /// </summary>
    public static class CrearVehiculoControlFactory
    {
        /// <summary>
        /// Crea un controlador de CrearVehiculo a partir
        /// de una instancia CrearVehiculoViewModel
        /// </summary>
        /// <param name="crearVehiculoViewModel"></param>
        /// <returns></returns>
        public static CrearVehiculoControl Create(CrearVehiculoViewModel crearVehiculoViewModel)
        {
            return new CrearVehiculoControl(crearVehiculoViewModel);
        }
    }
}
