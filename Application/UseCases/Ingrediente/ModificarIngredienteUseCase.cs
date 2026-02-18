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
            ValidarEntrada(ingredienteXModificar);

            var ingredienteActual = await _drivenIngredienteModificar.ObtenerPorReferencia(ingredienteXModificar.Ref);

            if (ingredienteActual == null)
            {
                throw new ArgumentException("El ingrediente no existe.");
            }

            bool duplicado = await _drivenIngredienteModificar.ExisteDuplicado(
                ingredienteXModificar.Ref,
                ingredienteXModificar.NameIngredient.Trim(),
                ingredienteActual.Id);

            if (duplicado)
            {
                throw new ArgumentException($"El ingrediente con referencia '{ingredienteXModificar.Ref}' o nombre '{ingredienteXModificar.NameIngredient}' ya existe.");
            }

            var modificadoIngrediente = await _drivenIngredienteModificar.ModificarIngrediente(new IngredienteDTODriver
            {
                Identidad = ingredienteActual.Id,
                Ref = ingredienteXModificar.Ref,
                NameIngredient = ingredienteXModificar.NameIngredient.Trim(),
                Quantity = ingredienteXModificar.Quantity,
                PrecioPack = ingredienteXModificar.PrecioPack,
                PrecioUnidad = ingredienteXModificar.PrecioUnidad
            });

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

        private void ValidarEntrada(IngredienteDTODriver dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.NameIngredient)) throw new ArgumentException("El nombre del ingrediente es obligatorio.", nameof(dto.NameIngredient));
            if (dto.Ref <= 0) throw new ArgumentException("La referencia debe ser un número positivo.", nameof(dto.Ref));
            if (dto.Quantity < 0) throw new ArgumentException("La cantidad no puede ser negativa.", nameof(dto.Quantity));
            if (dto.PrecioPack < 0) throw new ArgumentException("El precio del paquete no puede ser negativo.", nameof(dto.PrecioPack));
            if (dto.PrecioUnidad < 0) throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(dto.PrecioUnidad));
        }
    }
}
