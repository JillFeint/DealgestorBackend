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
        /// Obtiene un ingrediente por su referencia.
        /// </summary>
        /// <param name="referencia">Referencia del ingrediente.</param>
        /// <returns>El ingrediente encontrado o null si no existe.</returns>
        Task<Domain.Entities.Ingrediente?> ObtenerPorReferencia(int referencia);

        /// <summary>
        /// Verifica si existe otro ingrediente con la misma referencia o nombre (case-insensitive), excluyendo el Id actual.
        /// </summary>
        Task<bool> ExisteDuplicado(int referencia, string nombre, System.Guid excluirId);
        
        /// <summary>
        /// Modifica un ingrediente existente en la base de datos.
        /// </summary>
        /// <param name="ingredienteModificador">Los datos del ingrediente modificado.</param>
        /// <returns>El ingrediente modificado.</returns>
        Task<Domain.Entities.Ingrediente> ModificarIngrediente(IngredienteDTODriver ingredienteModificador);
    }
}
