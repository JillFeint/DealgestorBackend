using System.Threading.Tasks;
using Application.DTOs.Ingredientes;

namespace Application.Ports.DrivenPorts.Ingrediente
{
    /// <summary>
    /// Puerto para modificar ingredientes en la capa de persistencia.
    /// </summary>
    public interface PortDrivenIngredienteModificar
    {
        /// <summary>
        /// Obtiene el nombre del ingrediente por su referencia.
        /// </summary>
        /// <param name="ingredienteXModificar">Los datos del ingrediente a modificar.</param>
        /// <returns>True si se encontró el ingrediente, false en caso contrario.</returns>
        Task<bool> ObtenerNombreIngredienteRefe(IngredienteDTODriver ingredienteXModificar);
        
        /// <summary>
        /// Modifica un ingrediente existente en la base de datos.
        /// </summary>
        /// <param name="ingredienteModificador">Los datos del ingrediente modificado.</param>
        /// <returns>El ingrediente modificado.</returns>
        Task<Domain.Entities.Ingrediente> ModificarIngrediente(IngredienteDTODriver ingredienteModificador);
    }
}
