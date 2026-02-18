using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Roles;

namespace Application.Ports.DrivenPorts.Rol
{
    /// <summary>
    /// Puerto para consultar roles en la capa de persistencia.
    /// </summary>
    public interface PortDrivenRolConsultar
    {
        /// <summary>
        /// Consulta un rol por su nombre de forma asincrónica.
        /// </summary>
        /// <param name="nombre">El nombre del rol a buscar.</param>
        /// <returns>El rol encontrado o null si no existe.</returns>
        Task<Domain.Entities.Rol> ConsultarRolAsync(string nombre);
    }
}
