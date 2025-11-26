using System.Threading.Tasks;
using Application.DTOs.Ingredientes;

namespace Application.Ports.DrivenPorts.Ingrediente
{
    public interface PortDrivenIngredienteModificar
    {
        Task<bool> ObtenerNombreIngredienteRefe(IngredienteDTODriver ingredienteXModificar);
        Task<Domain.Entities.Ingrediente> ModificarIngrediente(IngredienteDTODriver ingredienteModificador);
    }
}
