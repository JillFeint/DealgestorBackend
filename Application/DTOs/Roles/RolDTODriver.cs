using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Roles
{
    /// <summary>
    /// DTO para la creación y transferencia de datos de roles desde el driver (API)
    /// </summary>
    public class RolDTODriver
    {
        /// <summary>
        /// Identificador único del rol. Se genera automáticamente si no se proporciona.
        /// </summary>
        public Guid Identidad { get; set; }

        /// <summary>
        /// Tipo de rol (ej: Admin, User, Manager)
        /// </summary>
        [Required(ErrorMessage = "El tipo de rol es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El tipo debe tener entre 3 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$", ErrorMessage = "El tipo solo puede contener letras, números y espacios.")]
        public string Tipe { get; set; } = string.Empty;

        /// <summary>
        /// Nombre descriptivo del rol
        /// </summary>
        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$", ErrorMessage = "El nombre solo puede contener letras, números y espacios.")]
        public string Name { get; set; } = string.Empty;

        public RolDTODriver() { }
    }
}