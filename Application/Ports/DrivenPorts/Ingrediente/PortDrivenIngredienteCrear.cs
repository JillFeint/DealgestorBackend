using Application.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Rol
{
    public interface PortDrivenIngredienteCrear
    {
        Task<bool> ExisteRolPorNombre(string nombre, string tipo);
        Task<Domain.Entities.Rol> CrearRol(RolDTODriver rol);
    }
}
