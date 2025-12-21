using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Ingredientes
{
    /// <summary>
    /// DTO para la creación y transferencia de datos de ingredientes desde el driver (API)
    /// </summary>
    public class IngredienteDTODriver
    {
        /// <summary>
        /// Identificador único del ingrediente. Se genera automáticamente si no se proporciona.
        /// </summary>
        public Guid Identidad { get; set; }

        /// <summary>
        /// Referencia numérica del ingrediente
        /// </summary>
        [Required(ErrorMessage = "La referencia del ingrediente es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La referencia debe ser un número positivo mayor a 0.")]
        public int Ref { get; set; }

        /// <summary>
        /// Nombre del ingrediente
        /// </summary>
        [Required(ErrorMessage = "El nombre del ingrediente es obligatorio.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 200 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s\-\.]+$", ErrorMessage = "El nombre solo puede contener letras, números, espacios, guiones y puntos.")]
        public string NameIngredient { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad disponible del ingrediente
        /// </summary>
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser un número positivo o cero.")]
        public int Quantity { get; set; }

        /// <summary>
        /// Precio del paquete del ingrediente en pesos colombianos (valor entero)
        /// </summary>
        [Required(ErrorMessage = "El precio del paquete es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio del paquete debe ser mayor o igual a 0.")]
        [DataType(DataType.Currency)]
        public decimal PrecioPack { get; set; }

        /// <summary>
        /// Precio unitario del ingrediente en pesos colombianos (valor entero)
        /// </summary>
        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor o igual a 0.")]
        [DataType(DataType.Currency)]
        public decimal PrecioUnidad { get; set; }
    }
}
