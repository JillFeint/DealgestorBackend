using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Perfil
{
    public interface PortDrivenPerfilCrear
    {
        Task<bool> ExistePerfilPorEmail(string email);
        Task<Domain.Entities.Perfil> CrearPerfil(PerfilIngresoDTODriver perfil);
    }
}