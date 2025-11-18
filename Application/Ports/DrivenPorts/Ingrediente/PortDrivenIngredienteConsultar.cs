using Application.DTOs.Ingredientes;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Ingrediente
{
    public interface PortDrivenIngredienteConsultar
    {
        Task<Domain.Entities.Ingrediente> ObtenerNombre(string nombreIngrediente);
    }
}