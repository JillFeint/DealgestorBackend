using Application.DTOs.Ingredientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Ingrediente
{
    /// <summary>
    /// Puerto para crear ingredientes desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverIngredienteCrear
    {
        /// <summary>
        /// Crea un nuevo ingrediente.
        /// </summary>
        /// <param name="nuevoIngredienteDTO">Los datos del ingrediente a crear.</param>
        /// <returns>El DTO del ingrediente creado.</returns>
        Task<IngredienteDTODriver> CrearNuevoIngrediente(IngredienteDTODriver nuevoIngredienteDTO);
    }
}
