using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.DTOs.Perfiles
{
    /// <summary>
    /// DTO para devolver información de perfil (salida de datos)
    /// </summary>
    public class PerfilRespuestaDTODriver
    {
        public string Email { get; set; } = string.Empty;
        public List<Negocio> Negocios { get; set; }
        public List<Rol> Roles { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

}