using Application.DTOs.Roles;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    public interface PortDrivenIngredienteModificar
    {
        Task<Domain.Entities.Rol> ObtenerRolPorNombreTipo(string name, string tipe);
        Task<Domain.Entities.Rol> ModificarRol(Domain.Entities.Rol rol);
    }
}
