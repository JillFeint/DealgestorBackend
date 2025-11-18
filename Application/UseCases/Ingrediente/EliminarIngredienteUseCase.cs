using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Rol
{
    public class EliminarIngredienteUseCase : PortDriverIngredienteEliminar
    {
        private readonly PortDrivenIngredienteEliminar _rolPortDrivenEliminar;

        public EliminarIngredienteUseCase(PortDrivenIngredienteEliminar rolPortEliminar)
        {
            _rolPortDrivenEliminar = rolPortEliminar;
        }

        public async Task<bool> EliminarRol(string nombre)
        {
            bool rolRespuestaEliminacion = await _rolPortDrivenEliminar.EliminarRol(nombre);

            return rolRespuestaEliminacion;
        }
    }
}
