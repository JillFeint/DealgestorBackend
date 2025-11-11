using Application.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    public interface PortDrivenRolModificar
    {
        Task<bool> ExisteRolPorId(Guid id);
        Task<RolDTODriven> ModificarRol(RolDTODriven rol);
    }
}
