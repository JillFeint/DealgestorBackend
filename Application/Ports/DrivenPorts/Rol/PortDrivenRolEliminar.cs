using System;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    /// <summary>
    /// Puerto para eliminar roles en la capa de persistencia.
    /// </summary>
    public interface PortDrivenRolEliminar
    {
        /// <summary>
        /// Obtiene un rol por su nombre (comparación normalizada).
        /// </summary>
        Task<Domain.Entities.Rol?> ObtenerRolPorNombre(string nombreNormalizado);

        /// <summary>
        /// Indica si el rol está en uso por algún perfil.
        /// </summary>
        Task<bool> EstaRolEnUso(Guid rolId);

        /// <summary>
        /// Elimina un rol de la base de datos por su nombre normalizado.
        /// </summary>
        /// <param name="nombre">El nombre del rol a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarRol(string nombre);
    }
}
