using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using System;
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
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

            string nombreCmp = nombre.Trim().ToUpperInvariant();

            var rol = await _rolPortDrivenEliminar.ObtenerRolPorNombre(nombreCmp);
            if (rol == null)
            {
                throw new ArgumentException("No se encontró ningún rol con el nombre especificado.", nameof(nombre));
            }

            if (await _rolPortDrivenEliminar.EstaRolEnUso(rol.Identificacion))
            {
                throw new InvalidOperationException("No se puede eliminar el rol porque está asignado a uno o más perfiles.");
            }

            return await _rolPortDrivenEliminar.EliminarRol(nombreCmp);
        }
    }
}
