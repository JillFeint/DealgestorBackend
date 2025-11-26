using Application.Ports.DrivenPorts.Ingrediente;
using Application.Ports.DriverPorts.Ingrediente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Ingrediente
{
    public class EliminarIngredienteUseCase : PortDriverIngredienteEliminar
    {
        private readonly PortDrivenIngredienteEliminar _ingredientePortDrivenEliminar;

        public EliminarIngredienteUseCase(PortDrivenIngredienteEliminar ingredientePortEliminar)
        {
            _ingredientePortDrivenEliminar = ingredientePortEliminar;
        }

        public async Task<bool> EliminarIngrediente(int referencia)
        {
            bool ingredienteRespuestaEliminacion = await _ingredientePortDrivenEliminar.EliminarIngrediente(referencia);

            return ingredienteRespuestaEliminacion;
        }
    }
}
