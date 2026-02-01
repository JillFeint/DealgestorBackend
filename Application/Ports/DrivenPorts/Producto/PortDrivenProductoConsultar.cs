using Application.DTOs.Productos;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Producto
{
    /// <summary>
    /// Puerto de salida para consultar productos en el repositorio
    /// </summary>
    public interface PortDrivenProductoConsultar
    {
        /// <summary>
        /// Consulta un producto por su nombre
        /// </summary>
        /// <param name="nombre">Nombre del producto a consultar</param>
        /// <returns>DTO del producto consultado</returns>
        Task<Domain.Entities.Producto> ConsultarProductoNombre(string nombre);
    }
}
