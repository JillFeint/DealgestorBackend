using Application.DTOs.Ingredientes;
using Application.Ports.DriverPorts.Ingrediente;
using Application.Ports.DrivenPorts.Ingrediente;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Ingrediente
{
    public class ConsultarIngredienteUseCase : PortDriverIngredienteConsultar
    {
        private readonly PortDrivenIngredienteConsultar _portDrivenIngredientes;

        public ConsultarIngredienteUseCase(PortDrivenIngredienteConsultar PortDrivenIngredientes)
        {
            _portDrivenIngredientes = PortDrivenIngredientes ?? throw new ArgumentNullException(nameof(PortDrivenIngredientes));
        }

        public async Task<IngredienteDTODriver> ConsultarIngredienteNombre(string nombre)
        {
            var ingredienteEntidad = await _portDrivenIngredientes.ObtenerPorNombre(nombre);

            if (ingredienteEntidad == null)
            {
                return null;
            }

            var ingredienteDTO = new IngredienteDTODriver
            {
                Id = ingredienteEntidad.Id,
                Referencia = ingredienteEntidad.Referencia,
                NombreIngrediente = ingredienteEntidad.NombreIngrediente,
                PrecioUnitario = ingredienteEntidad.PrecioUnitario
            };

            return ingredienteDTO;
        }
    }
}