using Application.DTOs.Ingredientes;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Ingrediente
{
    /// <summary>
    /// Puerto para consultar ingredientes desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverIngredienteConsultar
    {
        /// <summary>
        /// Consulta un ingrediente por su nombre.
        /// </summary>
        /// <param name="nombre">El nombre del ingrediente a buscar.</param>
        /// <returns>El DTO del ingrediente encontrado.</returns>
        Task<IngredienteDTODriver> ConsultarIngredienteNombre(string nombre);
    }
}