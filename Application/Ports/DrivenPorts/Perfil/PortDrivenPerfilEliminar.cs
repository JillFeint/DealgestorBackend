using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Perfil
{
    public interface PortDrivenPerfilEliminar
    {
        Task<Domain.Entities.Perfil> ObtenerPerfilPorEmail(string email);
        Task<bool> EliminarPerfilPorEmail(string email);
    }
}
