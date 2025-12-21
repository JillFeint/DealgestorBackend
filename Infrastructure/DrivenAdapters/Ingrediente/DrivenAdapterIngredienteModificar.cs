using Application.DTOs.Ingredientes;
using Application.Ports.DrivenPorts.Ingrediente;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Ingrediente
{
    /// <summary>
    /// Adaptador conducido para modificar ingredientes en la base de datos
    /// </summary>
    public class DrivenAdapterIngredienteModificar : PortDrivenIngredienteModificar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterIngredienteModificar> _logger;

        public DrivenAdapterIngredienteModificar(ApplicationDbContext dbContext, ILogger<DrivenAdapterIngredienteModificar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
       
        /// <summary>
        /// Verifica si un ingrediente existe y tiene todos los campos válidos
        /// </summary>
        /// <param name="ingredienteXModificar">DTO del ingrediente a verificar</param>
        /// <returns>True si existe y es válido, False en caso contrario</returns>
        public async Task<bool> ObtenerNombreIngredienteRefe(IngredienteDTODriver ingredienteXModificar)
        {
            try
            {
                _logger.LogInformation("Verificando ingrediente: Ref {Ref}", ingredienteXModificar?.Ref);

                if (ingredienteXModificar == null)
                {
                    _logger.LogWarning("Ingrediente nulo para verificación");
                    return false;
                }

                var ingVal = await _dbContext.tblIngredientes
                    .FirstOrDefaultAsync(r => r.tblReferencia == ingredienteXModificar.Ref);

                if (ingVal == null)
                {
                    _logger.LogWarning("Ingrediente no encontrado: Ref {Ref}", ingredienteXModificar.Ref);
                    return false;
                }

                var esValido =
                    !string.IsNullOrWhiteSpace(ingVal.tblNombreIngrediente) &&
                    ingVal.tblReferencia != null &&
                    ingVal.tblCantidad != null &&
                    ingVal.tblPrecioPaquete != null &&
                    ingVal.tblPrecioUnitario != null;

                if (!esValido)
                {
                    _logger.LogWarning("Ingrediente inválido o incompleto: Ref {Ref}", ingredienteXModificar.Ref);
                }
                else
                {
                    _logger.LogInformation("Ingrediente válido: Ref {Ref}", ingredienteXModificar.Ref);
                }

                return esValido;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar ingrediente: Ref {Ref}", ingredienteXModificar?.Ref);
                throw;
            }
        }

        /// <summary>
        /// Modifica un ingrediente existente en la base de datos
        /// </summary>
        /// <param name="ingredienteModificador">DTO del ingrediente a modificar</param>
        /// <returns>Entidad Ingrediente modificada o null si no existe</returns>
        public async Task<Domain.Entities.Ingrediente> ModificarIngrediente(IngredienteDTODriver ingredienteModificador)
        {
            try
            {
                _logger.LogInformation("Modificando ingrediente: {@Ingrediente}", 
                    new { ingredienteModificador.Ref, ingredienteModificador.NameIngredient });

                ArgumentNullException.ThrowIfNull(ingredienteModificador);

                var ingredienteEnDb = await _dbContext.tblIngredientes.FindAsync(ingredienteModificador.Ref);

                if (ingredienteEnDb == null)
                {
                    _logger.LogWarning("Ingrediente no encontrado para modificar: Ref {Ref}", ingredienteModificador.Ref);
                    return null;
                }

                ingredienteEnDb.tblReferencia = ingredienteModificador.Ref;
                ingredienteEnDb.tblNombreIngrediente = ingredienteModificador.NameIngredient;
                ingredienteEnDb.tblCantidad = ingredienteModificador.Quantity;
                ingredienteEnDb.tblPrecioPaquete = ingredienteModificador.PrecioPack;
                ingredienteEnDb.tblPrecioUnitario = ingredienteModificador.PrecioUnidad;

                _dbContext.tblIngredientes.Update(ingredienteEnDb);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Ingrediente modificado exitosamente: {@IngredienteModificado}", 
                    new { ingredienteEnDb.tblReferencia, ingredienteEnDb.tblNombreIngrediente });

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar ingrediente: {@Ingrediente}", 
                    new { ingredienteModificador.Ref, ingredienteModificador.NameIngredient });
                throw;
            }
        }
    }
}
