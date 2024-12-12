using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_Concesionario.Enums;
using WpfApp_Concesionario.Services;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario.Factory
{
    public static class CrearVehiculoViewModelFactory
    {
        public static CrearVehiculoViewModel Create(CocheService coche, ModoManipulacionDatos modoManipulacionDatos)
        {
            return new CrearVehiculoViewModel(coche, modoManipulacionDatos);
        }
    }
}
