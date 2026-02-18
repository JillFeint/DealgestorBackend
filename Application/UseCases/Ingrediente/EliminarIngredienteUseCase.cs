using Application.Ports.DrivenPorts.Ingrediente;
using Application.Ports.DriverPorts.Ingrediente;
using System;
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
            if (referencia <= 0)
                throw new ArgumentException("La referencia debe ser un número positivo.", nameof(referencia));

            bool eliminado = await _ingredientePortDrivenEliminar.EliminarIngrediente(referencia);

            if (!eliminado)
            {
                throw new ArgumentException("No se encontró ningún ingrediente con la referencia especificada.", nameof(referencia));
            }

            return eliminado;
        }
    }
}
