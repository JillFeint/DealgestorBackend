using Application.DTOs.Productos;
using System;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Producto
{
    /// <summary>
    /// Puerto para modificar productos en la capa de persistencia.
    /// </summary>
    public interface PortDrivenProductoModificar
    {
        /// <summary>
        /// Obtiene un producto por su referencia.
        /// </summary>
        Task<Domain.Entities.Producto?> ObtenerPorReferencia(int referencia);

        /// <summary>
        /// Verifica si existe otro producto con la misma referencia o nombre (case-insensitive), excluyendo el Id actual.
        /// </summary>
        Task<bool> ExisteDuplicado(int referencia, string nombre, Guid excluirId);

        /// <summary>
        /// Modifica un producto existente en la base de datos.
        /// </summary>
        /// <param name="productoModificador">Los datos del producto modificado.</param>
        /// <returns>El producto modificado.</returns>
        Task<Domain.Entities.Producto> ModificarProducto(ProductoDTODriver productoModificador);
    }
}