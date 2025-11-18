using Application.Ports.DrivenPorts.Ingrediente;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.DrivenAdapters.Ingrediente
{
    public class DrivenAdapterIngredienteConsultar : PortDrivenIngredienteConsultar
    {
        private readonly ApplicationDbContext _context;

        public DrivenAdapterIngredienteConsultar(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Domain.Entities.Ingrediente> ObtenerNombre(string nombreIngrediente)
        {        
            var tblIngrediente = await _context.tblIngredientes
             .Where(i => i.tblNombreIngrediente.ToLower().Contains(nombreIngrediente.ToLower()))
                .FirstOrDefaultAsync();

            if (tblIngrediente == null)
            {
                return null;
            }

            return new Domain.Entities.Ingrediente
            {
                Id = tblIngrediente.tblId,
                Referencia = tblIngrediente.tblReferencia,
                NombreIngrediente = tblIngrediente.tblNombreIngrediente,
                PrecioUnitario = tblIngrediente.tblPrecioUnitario  
            };
        }
    }
}