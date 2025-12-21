using Application.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    /// <summary>
    /// Puerto para crear roles en la capa de persistencia.
    /// </summary>
    public interface PortDrivenRolCrear
    {
        /// <summary>
        /// Verifica si existe un rol con el nombre y tipo especificados.
        /// </summary>
        /// <param name="nombre">El nombre del rol.</param>
        /// <param name="tipo">El tipo del rol.</param>
        /// <returns>True si el rol existe, false en caso contrario.</returns>
        Task<bool> ExisteRolPorNombre(string nombre, string tipo);
        
        /// <summary>
        /// Crea un nuevo rol en la base de datos.
        /// </summary>
        /// <param name="rol">Los datos del rol a crear.</param>
        /// <returns>El rol creado.</returns>
        Task<Domain.Entities.Rol> CrearRol(RolDTODriver rol);
    }
}
