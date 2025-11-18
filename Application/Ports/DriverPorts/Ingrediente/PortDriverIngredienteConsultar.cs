using Application.DTOs.Ingredientes;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Ingrediente
{
    public interface PortDriverIngredienteConsultar
    {
        Task<IngredienteDTODriver> ConsultarIngredienteNombre(string nombre);
    }
}