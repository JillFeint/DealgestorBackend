using Application.DTOs.Productos;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Producto
{
    /// <summary>
    /// Puerto de entrada para consultar productos
    /// </summary>
    public interface PortDriverProductoConsultar
    {
        /// <summary>
        /// Consulta un producto por su nombre
        /// </summary>
        /// <param name="nombre">Nombre del producto a consultar</param>
        /// <returns>DTO del producto consultado</returns>
        Task<ProductoDTODriver> ConsultarProductoNombre(string nombre);
    }
}
