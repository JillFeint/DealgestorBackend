using Application.DTOs.Roles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Rol
{
    public interface PortDriverIngredienteModificar
    {
        Task<RolDTODriver> ModificarRol(RolModificarRequestDTO rolModificarDTO);
    }
}
