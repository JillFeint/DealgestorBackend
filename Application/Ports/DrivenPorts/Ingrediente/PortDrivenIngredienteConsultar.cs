using Application.DTOs.Ingredientes;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Ingrediente
{
    /// <summary>
    /// Puerto para consultar ingredientes en la capa de persistencia.
    /// </summary>
    public interface PortDrivenIngredienteConsultar
    {
        /// <summary>
        /// Obtiene un ingrediente por su nombre.
        /// </summary>
        /// <param name="nombreIngrediente">El nombre del ingrediente a buscar.</param>
        /// <returns>El ingrediente encontrado o null si no existe.</returns>
        Task<Domain.Entities.Ingrediente> ObtenerNombre(string nombreIngrediente);
    }
}