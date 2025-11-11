using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    public interface IPortDrivenRolModificar
    {
        Task<Domain.Entities.Rol> ModificarRol(Domain.Entities.Rol rol);
    }
}
