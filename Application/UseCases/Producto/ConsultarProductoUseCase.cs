using Application.DTOs.Productos;
using Application.Ports.DrivenPorts.Producto;
using Application.Ports.DriverPorts.Producto;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.UseCases.Producto
{
    /// <summary>
    /// Caso de uso para consultar productos
    /// </summary>
    public class ConsultarProductoUseCase : PortDriverProductoConsultar
    {
        private readonly PortDrivenProductoConsultar _portDrivenProducto;

        public ConsultarProductoUseCase(PortDrivenProductoConsultar drivenPortProducto)
        {
            _portDrivenProducto = drivenPortProducto ?? throw new ArgumentNullException(nameof(drivenPortProducto));
        }

        /// <summary>
        /// Consulta un producto por su nombre
        /// </summary>
        /// <param name="nombre">Nombre del producto a consultar</param>
        /// <returns>DTO del producto consultado</returns>
        public async Task<ProductoDTODriver> ConsultarProductoNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del producto no puede estar vacío", nameof(nombre));
            }

            Domain.Entities.Producto producto = await _portDrivenProducto.ConsultarProductoNombre(nombre);

            if (producto == null)
            {
                return null;
            }

            return new ProductoDTODriver
            {
                Referencia = producto.Referencia,
                Nombre = producto.Nombre,
                Imagen = producto.Imagen,
                Categoria = producto.Categoria,
                PrecioSugerido = producto.PrecioSugerido,
                Costo = producto.Costo,
                IngredientesIds = producto.Ingredientes 
            };
        }
    }
}
