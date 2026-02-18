using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.DTOs.Perfiles
{
    /// <summary>
    /// DTO para devolver información de perfil (salida de datos hacia el cliente)
    /// </summary>
    public class PerfilRespuestaDTODriver
    {
        /// <summary>
        /// Email del usuario
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Lista de negocios asociados al perfil
        /// </summary>
        public List<Negocio> Negocios { get; set; } = new List<Negocio>();

        /// <summary>
        /// Lista de roles asociados al perfil
        /// </summary>
        public List<Rol> Roles { get; set; } = new List<Rol>();

        /// <summary>
        /// Fecha de creación del perfil
        /// </summary>
        public DateTime FechaCreacion { get; set; }
    }
}