using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Application.DTOs.Productos
{
    /// <summary>
    /// DTO para transferir información de producto desde el driven adapter (Base de datos)
    /// </summary>
    [Table("tblProductos")]
    public class ProductoDTODriven
    {
        /// <summary>
        /// Identificador único del producto
        /// </summary>
        [Key]
        public Guid tblId { get; set; }

        /// <summary>
        /// Número de referencia del producto
        /// </summary>
        public int tblReferencia { get; set; }

        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string tblNombre { get; set; }

        /// <summary>
        /// URL o ruta de la imagen del producto
        /// </summary>
        public string tblImagen { get; set; }

        /// <summary>
        /// Categoría del producto
        /// </summary>
        public string tblCategoria { get; set; }

        /// <summary>
        /// Precio sugerido del producto
        /// </summary>
        public int tblPrecioSugerido { get; set; }

        /// <summary>
        /// Costo del producto
        /// </summary>
        public int tblCosto { get; set; }

        public List<Domain.Entities.Ingrediente> tblIngredientesIds { get; set; }

        public ProductoDTODriven()
        {
        }
    }
}
