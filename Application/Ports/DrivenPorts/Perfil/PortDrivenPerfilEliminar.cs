using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Perfil
{
    /// <summary>
    /// Puerto para eliminar perfiles en la capa de persistencia.
    /// </summary>
    public interface PortDrivenPerfilEliminar
    {
        /// <summary>
        /// Obtiene un perfil por su email.
        /// </summary>
        /// <param name="email">El email del perfil a obtener.</param>
        /// <returns>El perfil encontrado o null si no existe.</returns>
        Task<Domain.Entities.Perfil> ObtenerPerfilPorEmail(string email);
        
        /// <summary>
        /// Elimina un perfil de la base de datos por su email.
        /// </summary>
        /// <param name="email">El email del perfil a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarPerfilPorEmail(string email);
    }
}
