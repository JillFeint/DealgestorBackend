using System;

namespace Application.DTOs.Ingredientes
{
    /// <summary>
    /// Entidad de base de datos para Ingrediente.
    /// Este DTO se mapea directamente a la tabla de ingredientes en la base de datos.
    /// </summary>
    public class IngredienteDTODriven
    {
        /// <summary>
        /// Identificador único del ingrediente
        /// </summary>
        public Guid tblId { get; set; }

        /// <summary>
        /// Referencia numérica del ingrediente
        /// </summary>
        public int tblReferencia { get; set; }

        /// <summary>
        /// Nombre del ingrediente
        /// </summary>
        public string tblNombreIngrediente { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad disponible del ingrediente
        /// </summary>
        public int tblCantidad { get; set; }

        /// <summary>
        /// Precio del paquete del ingrediente
        /// </summary>
        public decimal tblPrecioPaquete { get; set; }

        /// <summary>
        /// Precio unitario del ingrediente
        /// </summary>
        public decimal tblPrecioUnitario { get; set; }

        /// <summary>
        /// Constructor vacío requerido por EF Core
        /// </summary>
        public IngredienteDTODriven() { }

        /// <summary>
        /// Constructor con parámetros para inicialización
        /// </summary>
        public IngredienteDTODriven(Guid tblId, int tblReferencia, string tblNombreIngrediente, int tblCantidad, decimal tblPrecioUnitario, decimal tblPrecioPaquete)
        {
            this.tblId = tblId;
            this.tblReferencia = tblReferencia;
            this.tblNombreIngrediente = tblNombreIngrediente ?? string.Empty;
            this.tblCantidad = tblCantidad;
            this.tblPrecioPaquete = tblPrecioPaquete;
            this.tblPrecioUnitario = tblPrecioUnitario;
        }
    }
}

