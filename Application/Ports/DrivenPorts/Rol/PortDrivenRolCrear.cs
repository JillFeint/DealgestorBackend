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
        Task<bool> ExisteRolPorTipo(string nombre);
        Task<RolDTODriven> GuardarRol(RolDTODriven rol);
    }
}
