using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    /// <summary>
    /// Puerto para eliminar roles en la capa de persistencia.
    /// </summary>
    public interface PortDrivenRolEliminar
    {
        /// <summary>
        /// Elimina un rol de la base de datos por su nombre.
        /// </summary>
        /// <param name="nombre">El nombre del rol a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarRol(string nombre);
    }
}
