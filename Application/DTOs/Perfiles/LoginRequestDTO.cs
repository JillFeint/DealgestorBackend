using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Perfiles
{
    /// <summary>
    /// DTO para la solicitud de autenticación (login).
    /// Contiene las credenciales del usuario.
    /// </summary>
    public class LoginRequestDTO
    {
        /// <summary>
        /// Email del usuario que intenta autenticarse.
        /// </summary>
        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario en texto plano.
        /// IMPORTANTE: Este valor se transmite por HTTPS y nunca se almacena sin hashear.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string CodigoSecreto { get; set; } = string.Empty;
    }
}