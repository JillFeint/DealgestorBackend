using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Producto
{
    /// <summary>
    /// Puerto para eliminar productos desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverProductoEliminar
    {
        /// <summary>
        /// Elimina un producto por su referencia.
        /// </summary>
        /// <param name="referencia">La referencia del producto a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarProducto(int referencia);
    }
}