using Application.Ports.DrivenPorts.Ingrediente;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Ingrediente
{
    public class DrivenAdapterIngredienteEliminar : PortDrivenIngredienteEliminar
    {
        private readonly ApplicationDbContext _Context;

        public DrivenAdapterIngredienteEliminar(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        public async Task<bool> EliminarIngrediente(int referencia)
        {
            var ingrediente = await _Context.tblIngredientes.FirstOrDefaultAsync(r => r.tblReferencia == referencia);

            if (ingrediente == null)
            {
                return false;
            }

            _Context.tblIngredientes.Remove(ingrediente);
            var result = await _Context.SaveChangesAsync();
            return result > 0;
        }
    }
}