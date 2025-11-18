using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Rol
{
    public interface PortDriverIngredienteEliminar
    {
        Task<bool> EliminarRol(string nombre);
    }
}
