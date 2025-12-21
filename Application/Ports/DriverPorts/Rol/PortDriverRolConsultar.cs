using Application.DTOs.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Rol
{
    /// <summary>
    /// Puerto para consultar roles desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverRolConsultar
    {
        /// <summary>
        /// Consulta los identificadores de un rol por su nombre.
        /// </summary>
        /// <param name="Nombre">El nombre del rol a buscar.</param>
        /// <returns>El DTO del rol encontrado.</returns>
        Task<RolDTODriver> ConsultarIdentificadoresRol(string Nombre);
    }
}
