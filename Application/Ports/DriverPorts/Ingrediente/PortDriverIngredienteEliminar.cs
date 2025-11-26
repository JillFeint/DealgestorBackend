using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Ingrediente
{
    public interface PortDriverIngredienteEliminar
    {
        Task<bool> EliminarIngrediente(int ingrediente);
    }
}
