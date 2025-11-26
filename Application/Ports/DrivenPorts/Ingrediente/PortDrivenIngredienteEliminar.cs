using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Ingrediente
{
    public interface PortDrivenIngredienteEliminar
    {
        Task<bool> EliminarIngrediente(int referencia);
    }
}
