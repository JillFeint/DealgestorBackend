using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    /// <summary>
    /// Puerto para eliminar perfiles desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverPerfilEliminar
    {
        /// <summary>
        /// Elimina un perfil por su email con código especial.
        /// </summary>
        /// <param name="email">El email del perfil a eliminar.</param>
        /// <param name="codeEspecial">El código especial para la eliminación.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarPerfil(string email, string codeEspecial);
    }
}
