using Application.DTOs.Ingredientes;
using Application.Ports.DrivenPorts.Ingrediente;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Ingrediente
{
    public class DrivenAdapterIngredienteModificar : PortDrivenIngredienteModificar
    {
        private readonly ApplicationDbContext _dbContext;

        public DrivenAdapterIngredienteModificar(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
       
        public async Task<bool> ObtenerNombreIngredienteRefe(IngredienteDTODriver ingredienteXModificar)
        {
            if (ingredienteXModificar == null)
                return false;

            var ingVal = await _dbContext.tblIngredientes
                .FirstOrDefaultAsync(r => r.tblReferencia == ingredienteXModificar.Ref);

            if (ingVal == null)
                return false;

            return
                !string.IsNullOrWhiteSpace(ingVal.tblNombreIngrediente) &&
                ingVal.tblReferencia != null &&
                ingVal.tblCantidad != null &&
                ingVal.tblPrecioPaquete != null &&
                ingVal.tblPrecioUnitario != null;
        }

        public async Task<Domain.Entities.Ingrediente> ModificarIngrediente(IngredienteDTODriver ingredienteModificador)
        {
            ArgumentNullException.ThrowIfNull(ingredienteModificador);

            var ingredienteEnDb = await _dbContext.tblIngredientes.FindAsync(ingredienteModificador.Ref);

            if (ingredienteEnDb == null)
            {
                return null; 
            }

            ingredienteEnDb.tblReferencia = ingredienteModificador.Ref;
            ingredienteEnDb.tblNombreIngrediente = ingredienteModificador.NameIngredient;
            ingredienteEnDb.tblCantidad = ingredienteModificador.Quantity;
            ingredienteEnDb.tblPrecioPaquete = ingredienteModificador.PrecioPack;
            ingredienteEnDb.tblPrecioUnitario = ingredienteModificador.PrecioUnidad;

            _dbContext.tblIngredientes.Update(ingredienteEnDb);

            await _dbContext.SaveChangesAsync();

            return new Domain.Entities.Ingrediente
            {
                Id = ingredienteEnDb.tblId,
                Referencia = ingredienteEnDb.tblReferencia,
                NombreIngrediente = ingredienteEnDb.tblNombreIngrediente,
                Cantidad = ingredienteEnDb.tblCantidad,
                PrecioPaquete = ingredienteEnDb.tblPrecioPaquete,
                PrecioUnitario = ingredienteEnDb.tblPrecioUnitario
            };
        }
    }
}
