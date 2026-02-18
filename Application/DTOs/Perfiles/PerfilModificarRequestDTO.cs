using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Perfiles
{
    /// <summary>
    /// DTO para modificar un perfil existente en el sistema
    /// </summary>
    public class PerfilModificarRequestDTO
    {
        /// <summary>
        /// Email actual del perfil para identificarlo
        /// </summary>
        [Required(ErrorMessage = "El email actual es obligatorio para identificar el perfil.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [MaxLength(256, ErrorMessage = "El email no puede exceder 256 caracteres.")]
        public string EmailActual { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña actual para verificar identidad del usuario
        /// </summary>
        [Required(ErrorMessage = "El código secreto actual es obligatorio.")]
        [MaxLength(128, ErrorMessage = "El código secreto no puede exceder 128 caracteres.")]
        public string CodigoSecretoActual { get; set; } = string.Empty;

        /// <summary>
        /// Nuevo email (opcional, solo si se desea cambiar)
        /// </summary>
        [EmailAddress(ErrorMessage = "El formato del nuevo email no es válido.")]
        [MaxLength(256, ErrorMessage = "El nuevo email no puede exceder 256 caracteres.")]
        public string? NuevoEmail { get; set; }

        /// <summary>
        /// Nueva contraseña (opcional, solo si se desea cambiar)
        /// Debe contener al menos 8 caracteres, una mayúscula, una minúscula y un número
        /// </summary>
        [MinLength(8, ErrorMessage = "El nuevo código secreto debe tener al menos 8 caracteres.")]
        [MaxLength(128, ErrorMessage = "El nuevo código secreto no puede exceder 128 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula y un número.")]
        public string? NuevoCodigoSecreto { get; set; }
    }
}
