using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Perfiles
{
    public class PerfilModificarRequestDTO
    {
        [Required(ErrorMessage = "El email actual es obligatorio para identificar el perfil.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string EmailActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código secreto actual es obligatorio.")]
        public string CodigoSecretoActual { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El formato del nuevo email no es válido.")]
        public string? NuevoEmail { get; set; }

        [MinLength(7, ErrorMessage = "El nuevo código secreto debe tener al menos 7 caracteres.")]
        public string? NuevoCodigoSecreto { get; set; }
    }
}
