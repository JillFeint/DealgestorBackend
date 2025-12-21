using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Ingrediente
{
    /// <summary>
    /// Puerto para eliminar ingredientes desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverIngredienteEliminar
    {
        /// <summary>
        /// Elimina un ingrediente por su referencia.
        /// </summary>
        /// <param name="ingrediente">La referencia del ingrediente a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarIngrediente(int ingrediente);
    }
}
