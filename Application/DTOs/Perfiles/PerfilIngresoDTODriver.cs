using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.Perfiles
{
    /// <summary>
    /// DTO para ingreso/creación de perfil desde el cliente (API)
    /// </summary>
    public class PerfilIngresoDTODriver
    {
        /// <summary>
        /// Email del usuario (máximo 256 caracteres según RFC 5321)
        /// </summary>
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [MaxLength(256, ErrorMessage = "El email no puede exceder 256 caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario (mínimo 8 caracteres, debe contener mayúsculas, minúsculas y números)
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [MaxLength(128, ErrorMessage = "La contraseña no puede exceder 128 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", 
            ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula y un número.")]
        public string CodigoSecreto { get; set; } = string.Empty;

        public PerfilIngresoDTODriver() { }
    }       
}