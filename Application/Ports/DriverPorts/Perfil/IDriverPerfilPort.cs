using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Perfil
{
    public interface IDriverPerfilPort
    {
        Task<List<NegocioViewModel>> ObtenerNegociosDisponiblesParaPerfil(Guid perfilId);
    }
}
