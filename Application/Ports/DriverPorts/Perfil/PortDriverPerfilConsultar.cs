using Application.DTOs.Perfiles;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    public interface PortDriverPerfilConsultar
    {
        Task<PerfilRespuestaDTODriver> ConsultarPerfilPorEmail(string email, string codeEspecial);
    }
}