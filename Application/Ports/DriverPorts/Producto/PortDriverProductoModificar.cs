using Application.DTOs.Productos;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Producto
{
    /// <summary>
    /// Puerto para modificar productos desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverProductoModificar
    {
        /// <summary>
        /// Modifica un producto existente.
        /// </summary>
        /// <param name="productoXModificar">Los datos del producto a modificar.</param>
        /// <returns>El DTO del producto modificado.</returns>
        Task<ProductoDTODriver> ModificarProducto(ProductoDTODriver productoXModificar);
    }
}