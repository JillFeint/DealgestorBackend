using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Representa un perfil de usuario en el sistema.
    /// </summary>
    public class Perfil
    {
        /// <summary>
        /// Identificador único del perfil.
        /// </summary>
        public Guid Identidad { get; set; }

        /// <summary>
        /// Email del perfil de usuario.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Código secreto del perfil.
        /// </summary>
        public string CodigoSecreto { get; set; }

        /// <summary>
        /// Lista de negocios asociados al perfil.
        /// </summary>
        public List<Negocio> Negocios { get; set; }

        /// <summary>
        /// Lista de roles de permisos asociados al perfil.
        /// </summary>
        public List<Rol> PermisosRol { get; set; }

        /// <summary>
        /// Fecha de creación del perfil.
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Constructor vacío de la entidad Perfil.
        /// </summary>
        public Perfil() 
        { 
            Negocios = new List<Negocio>();
            PermisosRol = new List<Rol>();
        }
    }
}
