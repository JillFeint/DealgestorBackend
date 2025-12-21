using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Perfil
{
    /// <summary>
    /// Puerto para modificar perfiles en la capa de persistencia.
    /// </summary>
    public interface PortDrivenPerfilModificar
    {
        /// <summary>
        /// Obtiene un perfil por su email.
        /// </summary>
        /// <param name="email">El email del perfil a obtener.</param>
        /// <returns>El DTO del perfil encontrado o null si no existe.</returns>
        Task<PerfilDTODriven?> ObtenerPerfilPorEmail(string email);
        
        /// <summary>
        /// Modifica un perfil existente en la base de datos.
        /// </summary>
        /// <param name="perfil">El perfil con los datos modificados.</param>
        /// <returns>El DTO del perfil modificado.</returns>
        Task<PerfilDTODriven> ModificarPerfil(PerfilDTODriven perfil);
    }
}
