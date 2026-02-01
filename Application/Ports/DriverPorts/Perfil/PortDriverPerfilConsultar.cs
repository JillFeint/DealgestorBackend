using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    /// <summary>
    /// Puerto para consultar perfiles desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverPerfilConsultar
    {
        /// <summary>
        /// Consulta un perfil por su email con código especial.
        /// </summary>
        /// <param name="email">El email del perfil a buscar.</param>
        /// <param name="codeEspecial">El código especial para la consulta.</param>
        /// <returns>El DTO de respuesta del perfil encontrado.</returns>
        Task<PerfilRespuestaDTODriver> ConsultarPerfilPorEmail(string email);
    }
}