using Application.DTOs.Productos;
using Application.Ports.DrivenPorts.Producto;
using Application.Ports.DriverPorts.Producto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.Producto
{
    public class ModificarProductoUseCase : PortDriverProductoModificar
    {
        private readonly PortDrivenProductoModificar _drivenProductoModificar;

        public ModificarProductoUseCase(PortDrivenProductoModificar drivenProductoModificar)
        {
            _drivenProductoModificar = drivenProductoModificar ?? throw new ArgumentNullException(nameof(drivenProductoModificar));
        }

        public async Task<ProductoDTODriver> ModificarProducto(ProductoDTODriver productoXModificar)
        {
            ValidarEntrada(productoXModificar);

            var productoActual = await _drivenProductoModificar.ObtenerPorReferencia(productoXModificar.Referencia);

            if (productoActual == null)
            {
                throw new ArgumentException("El producto no existe.");
            }

            bool duplicado = await _drivenProductoModificar.ExisteDuplicado(
                productoXModificar.Referencia,
                productoXModificar.Nombre.Trim(),
                productoActual.Id);

            if (duplicado)
            {
                throw new ArgumentException($"El producto con referencia '{productoXModificar.Referencia}' o nombre '{productoXModificar.Nombre}' ya existe.");
            }

            var ingredientes = productoXModificar.IngredientesIds ?? new List<Ingrediente>();

            var productoModificado = await _drivenProductoModificar.ModificarProducto(new ProductoDTODriver
            {
                Referencia = productoXModificar.Referencia,
                Nombre = productoXModificar.Nombre.Trim(),
                Imagen = productoXModificar.Imagen,
                Categoria = productoXModificar.Categoria.Trim(),
                PrecioSugerido = productoXModificar.PrecioSugerido,
                Costo = productoXModificar.Costo,
                IngredientesIds = ingredientes
            });

            if (productoModificado == null)
            {
                throw new Exception("Error al modificar el producto en el repositorio.");
            }

            return new ProductoDTODriver
            {
                Referencia = productoModificado.Referencia,
                Nombre = productoModificado.Nombre,
                Imagen = productoModificado.Imagen,
                Categoria = productoModificado.Categoria,
                PrecioSugerido = productoModificado.PrecioSugerido,
                Costo = productoModificado.Costo,
                IngredientesIds = productoModificado.Ingredientes
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