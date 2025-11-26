using Application.DTOs.Ingredientes;
using Application.Ports.DrivenPorts.Ingrediente;
using Application.Ports.DriverPorts.Ingrediente;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Ingrediente
{
    public class ModificarIngredienteUseCase : PortDriverIngredienteModificar
    {
        private readonly PortDrivenIngredienteModificar _drivenIngredienteModificar;

        public ModificarIngredienteUseCase(PortDrivenIngredienteModificar drivenIngredienteModificar)
        {
            _drivenIngredienteModificar = drivenIngredienteModificar ?? throw new ArgumentNullException(nameof(drivenIngredienteModificar));
        }

        public async Task<IngredienteDTODriver> ModificarIngrediente(IngredienteDTODriver ingredienteXModificar)
        {
            var ingredienteAModificar = await _drivenIngredienteModificar.ObtenerNombreIngredienteRefe(ingredienteXModificar);

            if (ingredienteAModificar == null)
            {
                throw new Exception("El ingrediente no existe.");
            }

            if (ingredienteAModificar == false)
            {
                throw new Exception("El nombre o la referencia del no existe");
            }

            var modificadoIngrediente = await _drivenIngredienteModificar.ModificarIngrediente(ingredienteXModificar);

            if (modificadoIngrediente == null)
            {
                throw new Exception("Error al modificar el ingrediente en el repositorio.");
            }

            var resultaDTO = new IngredienteDTODriver    
            {
                Identidad = modificadoIngrediente.Id,
                Ref = modificadoIngrediente.Referencia,
                NameIngredient = modificadoIngrediente.NombreIngrediente,
                Quantity = modificadoIngrediente.Cantidad,
                PrecioPack = modificadoIngrediente.PrecioPaquete,
                PrecioUnidad = modificadoIngrediente.PrecioUnitario
            };

            return resultaDTO;
        }
    }
}
