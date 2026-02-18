using Application.DTOs.Productos;
using Application.Ports.DrivenPorts.Producto;
using Application.Ports.DriverPorts.Producto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.Producto
{
    /// <summary>
    /// Caso de uso para crear productos.
    /// </summary>
    public class CrearProductoUseCase : PortDriverProductoCrear
    {
        private readonly PortDrivenProductoCrear _productoPersistencePort;

        public CrearProductoUseCase(PortDrivenProductoCrear productoPersistencePort)
        {
            _productoPersistencePort = productoPersistencePort ?? throw new ArgumentNullException(nameof(productoPersistencePort));
        }

        public async Task<ProductoDTODriver> CrearNuevoProducto(ProductoDTODriver nuevoProducto)
        {
            ValidarEntrada(nuevoProducto);

            string nombreOriginal = nuevoProducto.Nombre.Trim();
            int referencia = nuevoProducto.Referencia;

            bool productoExiste = await _productoPersistencePort.ExisteProductoNombre(referencia, nombreOriginal);
           
            if (productoExiste)
            {
                throw new ArgumentException($"El producto con referencia '{referencia}' o nombre '{nombreOriginal}' ya existe en el sistema.");
            }

            Domain.Entities.Producto productoPersistido = await _productoPersistencePort.CrearProducto(nuevoProducto);

            return new ProductoDTODriver
            {
                Referencia = productoPersistido.Referencia,
                Nombre = productoPersistido.Nombre,
                Imagen = productoPersistido.Imagen,
                Categoria = productoPersistido.Categoria,
                PrecioSugerido = productoPersistido.PrecioSugerido,
                Costo = productoPersistido.Costo,
                IngredientesIds = productoPersistido.Ingredientes
            };
        }

        private void ValidarEntrada(ProductoDTODriver dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Nombre)) throw new ArgumentException("El nombre del producto es obligatorio.", nameof(dto.Nombre));
            if (string.IsNullOrWhiteSpace(dto.Categoria)) throw new ArgumentException("La categoría del producto es obligatoria.", nameof(dto.Categoria));
            if (dto.Referencia <= 0) throw new ArgumentException("La referencia debe ser un número positivo.", nameof(dto.Referencia));
            if (dto.PrecioSugerido < 0) throw new ArgumentException("El precio sugerido no puede ser negativo.", nameof(dto.PrecioSugerido));
            if (dto.Costo < 0) throw new ArgumentException("El costo no puede ser negativo.", nameof(dto.Costo));
        }
    }
}           