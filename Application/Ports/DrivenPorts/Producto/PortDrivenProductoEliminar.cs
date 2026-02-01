using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Producto
{
    /// <summary>
    /// Puerto para eliminar productos en la capa de persistencia.
    /// </summary>
    public interface PortDrivenProductoEliminar
    {
        /// <summary>
        /// Elimina un producto de la base de datos por su referencia.
        /// </summary>
        /// <param name="referencia">La referencia del producto a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarProducto(int referencia);
    }
}