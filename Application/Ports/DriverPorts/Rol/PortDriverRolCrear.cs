using Application.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Rol
{
    /// <summary>
    /// Puerto para crear roles desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverRolCrear
    {
        /// <summary>
        /// Crea un nuevo rol.
        /// </summary>
        /// <param name="nuevoRolDTO">Los datos del rol a crear.</param>
        /// <returns>El DTO del rol creado.</returns>
        Task<RolDTODriver> CrearNuevoRol(RolDTODriver nuevoRolDTO);
    }
}
