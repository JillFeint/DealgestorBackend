using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.DTOs.Perfiles
{
    /// <summary>
    /// Entidad de base de datos para Perfil.
    /// Este DTO se mapea directamente a la tabla de perfiles en la base de datos.
    /// IMPORTANTE: Nunca debe exponerse directamente al cliente (contiene hash de contraseña).
    /// </summary>
    public class PerfilDTODriven
    {
        /// <summary>
        /// Identificador único del perfil
        /// </summary>
        public Guid tblIdentidad { get; set; }

        /// <summary>
        /// Email del usuario
        /// </summary>
        public string tblEmail { get; set; }

        /// <summary>
        /// Hash de la contraseña (generado con Argon2id).
        /// Este campo contiene el hash, NO la contraseña en texto plano.
        /// NUNCA debe exponerse al cliente.
        /// </summary>
        public string tblCodigoSecreto { get; set; }

        /// <summary>
        /// Fecha de creación del perfil
        /// </summary>
        public DateTime tblFechaCreacion { get; set; }

        /// <summary>
        /// Lista de negocios asociados al perfil
        /// </summary>
        public List<Negocio> tblNegocios { get; set; }

        /// <summary>
        /// Lista de identificadores de roles/permisos asociados al perfil
        /// </summary>
        public List<Guid> tblPermisosRolIds { get; set; }

        public PerfilDTODriven() { }

        public PerfilDTODriven(Guid tblIdentidad, string tblEmail, string tblCodigoSecreto, DateTime tblFechaCreacion, List<Negocio> tblNegocios, List<Guid> tblPermisosRolIds)
        {
            this.tblIdentidad = tblIdentidad;
            this.tblEmail = tblEmail;
            this.tblCodigoSecreto = tblCodigoSecreto;
            this.tblFechaCreacion = tblFechaCreacion;
            this.tblNegocios = tblNegocios;
            this.tblPermisosRolIds = tblPermisosRolIds;
        }
    }
}