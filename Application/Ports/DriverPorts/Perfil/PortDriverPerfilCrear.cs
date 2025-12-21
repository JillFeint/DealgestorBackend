using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    public interface PortDriverPerfilCrear
    {
        Task<PerfilRespuestaDTODriver> CrearNuevoPerfil(PerfilIngresoDTODriver nuevoPerfilDTO);
    }
}