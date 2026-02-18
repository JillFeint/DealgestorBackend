using Application.DTOs.Productos;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Producto
{
    /// <summary>
    /// Puerto de entrada para crear productos.
    /// </summary>
    public interface PortDriverProductoCrear
    {
        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="nuevoProducto">Datos del producto a crear.</param>
        /// <returns>DTO del producto creado.</returns>
        Task<ProductoDTODriver> CrearNuevoProducto(ProductoDTODriver nuevoProducto);
    }
}