using Application.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    public interface PortDrivenRolCrear
    {
        Task<bool> ExisteRolPorNombre(string nombre);
        Task<RolDTODriven> CrearRol(RolDTODriven rol);
    }
}
