using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    /// <summary>
    /// Puerto para crear perfiles desde la capa de aplicación/drivers.
    /// </summary>
    public interface PortDriverPerfilCrear
    {
        /// <summary>
        /// Crea un nuevo perfil.
        /// </summary>
        /// <param name="nuevoPerfilDTO">Los datos del perfil a crear.</param>
        /// <returns>El DTO de respuesta del perfil creado.</returns>
        Task<PerfilRespuestaDTODriver> CrearNuevoPerfil(PerfilIngresoDTODriver nuevoPerfilDTO);
    }
}