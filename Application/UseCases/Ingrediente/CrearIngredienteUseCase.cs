using Application.DTOs.Ingredientes;
using Application.Ports.DrivenPorts.Ingrediente;
using Application.Ports.DriverPorts.Ingrediente;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Ingrediente
{
    public class CrearIngredienteUseCase : PortDriverIngredienteCrear
    {
        private readonly PortDrivenIngredienteCrear _ingredientePersistencePort;

        public CrearIngredienteUseCase(PortDrivenIngredienteCrear ingredientePersistencePort)
        {
            _ingredientePersistencePort = ingredientePersistencePort;
        }

        public async Task<IngredienteDTODriver> CrearNuevoIngrediente(IngredienteDTODriver nuevoIngredienteDTO)
        {
            ValidarEntrada(nuevoIngredienteDTO);

            string nombreOriginal = nuevoIngredienteDTO.NameIngredient.Trim();
            int referencia = nuevoIngredienteDTO.Ref;

            bool ingredienteExiste = await _ingredientePersistencePort.ExisteIngredienteNombre(referencia, nombreOriginal);
            if (ingredienteExiste)
            {
                throw new ArgumentException($"El ingrediente con referencia '{referencia}' o nombre '{nombreOriginal}' ya existe en el sistema.");
            }

            Domain.Entities.Ingrediente ingredientePersistido = await _ingredientePersistencePort.CrearIngrediente(new IngredienteDTODriver
            {
                Identidad = nuevoIngredienteDTO.Identidad,
                Ref = referencia,
                NameIngredient = nombreOriginal,
                Quantity = nuevoIngredienteDTO.Quantity,
                PrecioPack = nuevoIngredienteDTO.PrecioPack,
                PrecioUnidad = nuevoIngredienteDTO.PrecioUnidad
            });

            var ingredienteCreadoDTO = new IngredienteDTODriver
            {
                Identidad = ingredientePersistido.Id,
                Ref = ingredientePersistido.Referencia,
                NameIngredient = ingredientePersistido.NombreIngrediente,
                Quantity = ingredientePersistido.Cantidad,
                PrecioPack = ingredientePersistido.PrecioPaquete,
                PrecioUnidad = ingredientePersistido.PrecioUnitario
            };

            return ingredienteCreadoDTO;
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