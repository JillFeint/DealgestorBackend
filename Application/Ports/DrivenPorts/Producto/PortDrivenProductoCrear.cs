using Application.DTOs.Productos;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Producto
{
    /// <summary>
    /// Puerto de salida para crear productos en la capa de persistencia.
    /// </summary>
    public interface PortDrivenProductoCrear
    {
        /// <summary>
        /// Verifica si existe un producto con la referencia o el nombre especificados.
        /// </summary>
        /// <param name="referencia">Referencia del producto.</param>
        /// <param name="nombre">Nombre del producto.</param>
        /// <returns>True si existe, false en caso contrario.</returns>
        Task<bool> ExisteProductoNombre(int referencia, string nombre);

        /// <summary>
        /// Crea un nuevo producto en la base de datos.
        /// </summary>
        /// <param name="producto">Datos del producto a crear.</param>
        /// <returns>Entidad Producto creada.</returns>
        Task<Domain.Entities.Producto> CrearProducto(ProductoDTODriver producto);
    }
}