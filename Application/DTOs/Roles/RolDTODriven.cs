using System;

namespace Application.DTOs.Roles
{
    /// <summary>
    /// Entidad de base de datos para Rol.
    /// Este DTO se mapea directamente a la tabla de roles en la base de datos.
    /// </summary>
    public class RolDTODriven
    {
        /// <summary>
        /// Identificador único del rol
        /// </summary>
        public Guid tblIdentificacion { get; set; }

        /// <summary>
        /// Tipo de rol
        /// </summary>
        public string tblTipo { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del rol
        /// </summary>
        public string tblNombre { get; set; } = string.Empty;

        /// <summary>
        /// Constructor vacío requerido por EF Core
        /// </summary>
        public RolDTODriven() { }

        /// <summary>
        /// Constructor con parámetros para inicialización
        /// </summary>
        public RolDTODriven(Guid tblIdentificacion, string tblTipo, string tblNombre)
        {
            this.tblIdentificacion = tblIdentificacion;
            this.tblTipo = tblTipo ?? string.Empty;
            this.tblNombre = tblNombre ?? string.Empty;
        }
    }
}