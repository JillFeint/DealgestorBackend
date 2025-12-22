using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Representa un ingrediente en el sistema.
    /// </summary>
    public class Ingrediente
    {
        /// <summary>
        /// Identificador único del ingrediente.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Número de referencia del ingrediente.
        /// </summary>
        public int Referencia { get; set; }

        /// <summary>
        /// Nombre del ingrediente.
        /// </summary>
        public string NombreIngrediente { get; set; }

        /// <summary>
        /// Cantidad disponible del ingrediente.
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// Precio del paquete del ingrediente.
        /// </summary>
        public decimal PrecioPaquete { get; set; }

        /// <summary>
        /// Precio unitario del ingrediente.
        /// </summary>
        public decimal PrecioUnitario { get; set; }

        /// <summary>
        /// Constructor vacío de la entidad Ingrediente.
        /// </summary>
        public Ingrediente() { }
    }
}
