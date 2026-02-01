using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    /// <summary>
    /// Puerto conductor para la autenticación de perfiles.
    /// Define el contrato para el caso de uso de autenticación (login).
    /// </summary>
    public interface PortDriverPerfilAutenticar
    {
        /// <summary>
        /// Autentica un perfil de usuario mediante sus credenciales.
        /// Valida el email y contraseña, y genera un token JWT si las credenciales son correctas.
        /// </summary>
        /// <param name="credenciales">Credenciales del usuario (email y contraseña)</param>
        /// <returns>
        /// LoginResponseDTO conteniendo el token JWT, email, roles y fecha de expiración.
        /// </returns>
        /// <exception cref="ArgumentException">Si las credenciales son inválidas o el usuario no existe</exception>
        /// <exception cref="UnauthorizedAccessException">Si la contraseña es incorrecta</exception>
        Task<LoginResponseDTO> AutenticarPerfil(LoginRequestDTO credenciales);
    }
}
