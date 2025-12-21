using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Perfil
{
    /// <summary>
    /// Puerto para crear perfiles en la capa de persistencia.
    /// </summary>
    public interface PortDrivenPerfilCrear
    {
        /// <summary>
        /// Verifica si existe un perfil con el email especificado.
        /// </summary>
        /// <param name="email">El email del perfil.</param>
        /// <returns>True si el perfil existe, false en caso contrario.</returns>
        Task<bool> ExistePerfilPorEmail(string email);
        
        /// <summary>
        /// Crea un nuevo perfil en la base de datos.
        /// </summary>
        /// <param name="perfil">Los datos del perfil a crear.</param>
        /// <returns>El perfil creado.</returns>
        Task<Domain.Entities.Perfil> CrearPerfil(PerfilIngresoDTODriver perfil);
    }
}