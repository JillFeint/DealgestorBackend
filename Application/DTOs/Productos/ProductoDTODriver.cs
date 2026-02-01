using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Productos
{
    /// <summary>
    /// DTO para transferir información de producto desde y hacia el driver
    /// </summary>
    public class ProductoDTODriver
    {
        /// <summary>
        /// Número de referencia del producto
        /// </summary>
        [Required(ErrorMessage = "La referencia del producto es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La referencia debe ser un número positivo")]
        public int Referencia { get; set; }

        /// <summary>
        /// Nombre del producto
        /// </summary>
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
        public string Nombre { get; set; }

        /// <summary>
        /// URL o ruta de la imagen del producto
        /// </summary>
        [StringLength(500, ErrorMessage = "La URL de la imagen no puede exceder 500 caracteres")]
        public string Imagen { get; set; }

        /// <summary>
        /// Categoría del producto
        /// </summary>
        [Required(ErrorMessage = "La categoría del producto es obligatoria")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La categoría no puede exceder 100 caracteres")]
        public string Categoria { get; set; }

        /// <summary>
        /// Precio sugerido del producto
        /// </summary>
        [Required(ErrorMessage = "El precio sugerido es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El precio sugerido debe ser un número positivo o cero")]
        public int PrecioSugerido { get; set; }

        /// <summary>
        /// Costo del producto
        /// </summary>
        [Required(ErrorMessage = "El costo es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El costo debe ser un número positivo o cero")]
        public int Costo { get; set; }

        /// <summary>
        /// Lista de IDs de ingredientes que componen el producto
        /// </summary>
        public List<Domain.Entities.Ingrediente> IngredientesIds { get; set; }

        public ProductoDTODriver()
        {
        }
    }
}
