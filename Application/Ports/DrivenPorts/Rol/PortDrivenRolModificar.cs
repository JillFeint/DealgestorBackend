using Application.DTOs.Roles;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    /// <summary>
    /// Puerto para modificar roles en la capa de persistencia.
    /// </summary>
    public interface PortDrivenRolModificar
    {
        /// <summary>
        /// Obtiene un rol por su nombre y tipo.
        /// </summary>
        /// <param name="name">El nombre del rol.</param>
        /// <param name="tipe">El tipo del rol.</param>
        /// <returns>El rol encontrado o null si no existe.</returns>
        Task<Domain.Entities.Rol> ObtenerRolNombreTipo(string name, string tipe);
        
        /// <summary>
        /// Modifica un rol existente en la base de datos.
        /// </summary>
        /// <param name="rol">El rol con los datos modificados.</param>
        /// <returns>El rol modificado.</returns>
        Task<Domain.Entities.Rol> ModificarRol(Domain.Entities.Rol rol);
    }
}
