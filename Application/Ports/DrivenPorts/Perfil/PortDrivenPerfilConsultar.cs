using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Perfil
{
    /// <summary>
    /// Puerto para consultar perfiles en la capa de persistencia.
    /// </summary>
    public interface PortDrivenPerfilConsultar
    {
        /// <summary>
        /// Consulta un perfil por su email de forma asincrónica.
        /// </summary>
        /// <param name="email">El email del perfil a buscar.</param>
        /// <returns>El perfil encontrado o null si no existe.</returns>
        Task<Domain.Entities.Perfil> ConsultarPerfilPorEmailAsync(string email);
    }
}