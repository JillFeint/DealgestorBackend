using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Representa un producto en el sistema.
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Número de referencia del producto.
        /// </summary>
        public int Referencia { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// URL o ruta de la imagen del producto.
        /// </summary>
        public string Imagen { get; set; }

        /// <summary>
        /// Categoría del producto.
        /// </summary>
        public string Categoria { get; set; }

        /// <summary>
        /// Precio sugerido del producto.
        /// </summary>
        public int PrecioSugerido { get; set; }

        /// <summary>
        /// Costo del producto.
        /// </summary>
        public int Costo { get; set; }

        /// <summary>
        /// Lista de ingredientes que componen el producto.
        /// </summary>
        public List<Ingrediente> Ingredientes { get; set; }

        /// <summary>
        /// Constructor vacío de la entidad Producto.
        /// </summary>
        public Producto() 
        { 
            Ingredientes = new List<Ingrediente>();
        }
    }
}
