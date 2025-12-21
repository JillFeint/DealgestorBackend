using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Perfil
{
    public interface PortDrivenPerfilModificar
    {
        Task<PerfilDTODriven?> ObtenerPerfilPorEmail(string email);
        Task<PerfilDTODriven> ModificarPerfil(PerfilDTODriven perfil);
    }
}
