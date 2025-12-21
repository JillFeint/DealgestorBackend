using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    public interface PortDriverPerfilEliminar
    {
        Task<bool> EliminarPerfil(string email, string codeEspecial);
    }
}
