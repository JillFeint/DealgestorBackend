using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Rol
{
    /// <summary>
    /// Puerto para eliminar roles desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverRolEliminar
    {
        /// <summary>
        /// Elimina un rol por su nombre.
        /// </summary>
        /// <param name="nombre">El nombre del rol a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarRol(string nombre);
    }
}
