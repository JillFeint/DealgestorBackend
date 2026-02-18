using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    /// <summary>
    /// Puerto para modificar perfiles desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverPerfilModificar
    {
        /// <summary>
        /// Modifica un perfil existente.
        /// </summary>
        /// <param name="perfilModificarDTO">Los datos del perfil a modificar.</param>
        /// <returns>El DTO de respuesta del perfil modificado.</returns>
        Task<PerfilRespuestaDTODriver> ModificarPerfil(PerfilModificarRequestDTO perfilModificarDTO);
    }
}
