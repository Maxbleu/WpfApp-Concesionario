using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_Concesionario.Controls;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario.Factory
{
    public static class CrearVehiculoControlFactory
    {
        public static CrearVehiculoControl Create(CrearVehiculoViewModel crearVehiculoViewModel)
        {
            return new CrearVehiculoControl(crearVehiculoViewModel);
        }
    }
}
