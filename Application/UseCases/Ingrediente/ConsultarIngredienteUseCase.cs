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
            var ingredienteEntidad = await _portDrivenIngredientes.ObtenerNombre(nombre);

            if (ingredienteEntidad == null)
            {
                return null;
            }

            var ingredienteDTO = new IngredienteDTODriver
            {
                Identidad = ingredienteEntidad.Id,
                Ref = ingredienteEntidad.Referencia,
                NameIngredient = ingredienteEntidad.NombreIngrediente,
                Quantity = ingredienteEntidad.Cantidad,
                PrecioPack = ingredienteEntidad.PrecioPaquete,
                PrecioUnidad = ingredienteEntidad.PrecioUnitario
            };

            return ingredienteDTO;
        }
    }
}