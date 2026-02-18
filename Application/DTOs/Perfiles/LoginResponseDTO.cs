using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.DTOs.Perfiles
{
    /// <summary>
    /// DTO para la respuesta de autenticación exitosa (login).
    /// Contiene el token JWT y la información del usuario autenticado.
    /// </summary>
    public class LoginResponseDTO
    {
        /// <summary>
        /// Token JWT generado para el usuario autenticado.
        /// Este token debe ser incluido en el header Authorization de las siguientes peticiones.
        /// Formato: Bearer {token}
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Email del usuario autenticado.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Lista de nombres de roles asociados al perfil.
        /// Ejemplo: ["Admin", "Vendedor"]
        /// </summary>
        public List<Rol> Roles { get; set; } = new List<Rol>();

        /// <summary>
        /// Fecha y hora (UTC) en la que expira el token.
        /// Después de esta fecha el token ya no será válido y el usuario deberá autenticarse nuevamente.
        /// </summary>
        public DateTime FechaExpiracion { get; set; }
    }
}
