using Application.DTOs.Ingredientes;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Ingrediente
{
    /// <summary>
    /// Puerto para modificar ingredientes desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverIngredienteModificar
    {
        /// <summary>
        /// Modifica un ingrediente existente.
        /// </summary>
        /// <param name="ingredienteXModificar">Los datos del ingrediente a modificar.</param>
        /// <returns>El DTO del ingrediente modificado.</returns>
        Task<IngredienteDTODriver> ModificarIngrediente(IngredienteDTODriver ingredienteXModificar);
    }
}
