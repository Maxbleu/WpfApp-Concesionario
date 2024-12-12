using WpfApp_Concesionario.Enums;
using WpfApp_Concesionario.Services;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario.Factory
{
    /// <summary>
    /// Aplicamos el patron Factory para crear el view model
    /// ya que, require de un ViewModel del servicio CocheService
    /// pero a su vez, requiere de un modo de uso que solo sabremos
    /// en tiempo de ejecución.
    /// 
    /// Adaptandose este patron a las necesidades de la aplicacion.
    /// </summary>
    public static class CrearVehiculoViewModelFactory
    {
        /// <summary>
        /// Crear un ViewModel de CrearVehiculo a partir de un servicio CocheService
        /// y un modo de manipulación de los datos
        /// </summary>
        /// <param name="coche"></param>
        /// <param name="modoManipulacionDatos"></param>
        /// <returns></returns>
        public static CrearVehiculoViewModel Create(CocheService coche, ModoManipulacionDatos modoManipulacionDatos)
        {
            return new CrearVehiculoViewModel(coche, modoManipulacionDatos);
        }
    }
}
