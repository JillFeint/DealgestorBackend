using Application.DTOs.Ingredientes;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Ingrediente
{
    public interface PortDriverIngredienteModificar
    {
        Task<IngredienteDTODriver> ModificarIngrediente(IngredienteDTODriver ingredienteXModificar);
    }
}
