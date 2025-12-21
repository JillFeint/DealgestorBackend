using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    public interface PortDriverPerfilModificar
    {
        Task<PerfilRespuestaDTODriver> ModificarPerfil(PerfilModificarRequestDTO perfilModificarDTO);
    }
}
