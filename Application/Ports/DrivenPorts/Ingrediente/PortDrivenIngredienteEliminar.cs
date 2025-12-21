using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Ingrediente
{
    /// <summary>
    /// Puerto para eliminar ingredientes en la capa de persistencia.
    /// </summary>
    public interface PortDrivenIngredienteEliminar
    {
        /// <summary>
        /// Elimina un ingrediente de la base de datos por su referencia.
        /// </summary>
        /// <param name="referencia">La referencia del ingrediente a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarIngrediente(int referencia);
    }
}
