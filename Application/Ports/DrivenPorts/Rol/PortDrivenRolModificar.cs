using Application.DTOs.Roles;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    public interface PortDrivenRolModificar
    {
        Task<Domain.Entities.Rol> ObtenerRolNombreTipo(string name, string tipe);
        Task<Domain.Entities.Rol> ModificarRol(Domain.Entities.Rol rol);
    }
}
