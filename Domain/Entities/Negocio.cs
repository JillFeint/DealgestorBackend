using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Representa un negocio en el sistema.
    /// </summary>
    public class Negocio
    {
        /// <summary>
        /// Identificador único del negocio.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Número de referencia del negocio.
        /// </summary>
        public int Referencia { get; set; }

        /// <summary>
        /// Nombre del negocio.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción del negocio.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Tipo de negocio.
        /// </summary>
        public string TipoNegocio { get; set; }

        /// <summary>
        /// Lista de productos asociados al negocio.
        /// </summary>
        public List<Producto> Productos { get; set; }

        /// <summary>
        /// Constructor vacío de la entidad Negocio.
        /// </summary>
        public Negocio() 
        { 
            Productos = new List<Producto>();
        }
    }
}
