using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Roles
{
    /// <summary>
    /// DTO para modificar un rol existente en el sistema
    /// </summary>
    public class RolModificarRequestDTO
    {
        /// <summary>
        /// Nombre actual del rol a modificar (para identificarlo)
        /// </summary>
        [Required(ErrorMessage = "El nombre actual del rol es obligatorio para identificarlo.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre actual debe tener entre 3 y 100 caracteres.")]
        public string NombreActual { get; set; } = string.Empty;

        /// <summary>
        /// Tipo actual del rol a modificar (para identificarlo)
        /// </summary>
        [Required(ErrorMessage = "El tipo actual del rol es obligatorio para identificarlo.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El tipo actual debe tener entre 3 y 50 caracteres.")]
        public string TipoActual { get; set; } = string.Empty;

        /// <summary>
        /// Nuevo nombre para el rol (opcional, solo si se desea cambiar)
        /// </summary>
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nuevo nombre debe tener entre 3 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$", ErrorMessage = "El nuevo nombre solo puede contener letras, números y espacios.")]
        public string? NuevoNombre { get; set; }

        /// <summary>
        /// Nuevo tipo para el rol (opcional, solo si se desea cambiar)
        /// </summary>
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nuevo tipo debe tener entre 3 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$", ErrorMessage = "El nuevo tipo solo puede contener letras, números y espacios.")]
        public string? NuevoTipo { get; set; }
    }
}
