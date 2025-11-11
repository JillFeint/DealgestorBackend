using Application.DTOs.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Rol
{
    public interface PortDriverRolConsultar
    {
        Task<RolDTODriver> ConsultarIdentificadoresRol(string Nombre);
    }
}
