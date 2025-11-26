using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Rol
{
    public class EliminarRolUseCase : PortDriverRolEliminar
    {
        private readonly PortDrivenRolEliminar _rolPortDrivenEliminar;

        public EliminarRolUseCase(PortDrivenRolEliminar rolPortEliminar)
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
