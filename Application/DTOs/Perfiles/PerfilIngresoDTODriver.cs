using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.Perfiles
{
    public class PerfilIngresoDTODriver
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(7, ErrorMessage = "La contraseña debe tener al menos 7 caracteres.")]
        public string CodigoSecreto { get; set; } = string.Empty;

        public PerfilIngresoDTODriver() { }
    }       
}