using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Perfil
{
    public interface PortDrivenPerfilConsultar
    {
        Task<Domain.Entities.Perfil> ConsultarPerfilPorEmailAsync(string email);
    }
}