using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Roles;

namespace Application.Ports.DrivenPorts.Rol
{
    public interface PortDrivenRolConsultar
    {
        Task<Domain.Entities.Rol> ConsultarRolAsync(string nombre);
    }
}
