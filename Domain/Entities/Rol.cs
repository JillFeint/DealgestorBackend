using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Representa un rol en el sistema.
    /// </summary>
    public class Rol
    {
        /// <summary>
        /// Identificador único del rol.
        /// </summary>
        public Guid Identificacion { get; set; }

        /// <summary>
        /// Tipo de rol.
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Nombre del rol.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Constructor vacío de la entidad Rol.
        /// </summary>
        public Rol() { }

        /// <summary>
        /// Constructor que inicializa el rol con sus valores.
        /// </summary>
        /// <param name="Identificacion">El identificador único del rol.</param>
        /// <param name="Tipo">El tipo de rol.</param>
        /// <param name="Nombre">El nombre del rol.</param>
        public Rol(Guid Identificacion, string Tipo, string Nombre)
        {
            this.Identificacion = Identificacion;
            this.Tipo = Tipo;
            this.Nombre = Nombre;
        }
    }
}
