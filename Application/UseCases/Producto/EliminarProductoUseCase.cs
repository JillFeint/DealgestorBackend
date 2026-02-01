using Application.Ports.DrivenPorts.Producto;
using Application.Ports.DriverPorts.Producto;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Producto
{
    public class EliminarProductoUseCase : PortDriverProductoEliminar
    {
        private readonly PortDrivenProductoEliminar _productoPortDrivenEliminar;

        public EliminarProductoUseCase(PortDrivenProductoEliminar productoPortEliminar)
        {
            _productoPortDrivenEliminar = productoPortEliminar;
        }

        public async Task<bool> EliminarProducto(int referencia)
        {
            if (referencia <= 0)
                throw new ArgumentException("La referencia debe ser un número positivo.", nameof(referencia));

            bool eliminado = await _productoPortDrivenEliminar.EliminarProducto(referencia);

            if (!eliminado)
            {
                throw new ArgumentException("No se encontró ningún producto con la referencia especificada.", nameof(referencia));
            }

            return eliminado;
        }
    }
}