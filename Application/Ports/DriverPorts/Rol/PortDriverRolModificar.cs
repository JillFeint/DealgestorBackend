using Application.DTOs.Roles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Rol
{
    public interface PortDriverRolModificar
    {
        Task<RolDTODriver> ModificarRol(RolDTODriver rolModificarDTO);
    }
}
