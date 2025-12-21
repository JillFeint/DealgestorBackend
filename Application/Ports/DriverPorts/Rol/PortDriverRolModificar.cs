using Application.DTOs.Roles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Rol
{
    /// <summary>
    /// Puerto para modificar roles desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverRolModificar
    {
        /// <summary>
        /// Modifica un rol existente.
        /// </summary>
        /// <param name="rolModificarDTO">Los datos del rol a modificar.</param>
        /// <returns>El DTO del rol modificado.</returns>
        Task<RolDTODriver> ModificarRol(RolModificarRequestDTO rolModificarDTO);
    }
}
