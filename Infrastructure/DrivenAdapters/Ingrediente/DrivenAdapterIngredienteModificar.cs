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

        public async Task<Domain.Entities.Ingrediente?> ObtenerPorReferencia(int referencia)
        {
            try
            {
                _logger.LogInformation("Obteniendo ingrediente por referencia: {Ref}", referencia);

                var ing = await _dbContext.tblIngredientes.FirstOrDefaultAsync(r => r.tblReferencia == referencia);

                if (ing == null)
                {
                    _logger.LogWarning("Ingrediente no encontrado: Ref {Ref}", referencia);
                    return null;
                }

                return new Domain.Entities.Ingrediente
                {
                    Id = ing.tblId,
                    Referencia = ing.tblReferencia,
                    NombreIngrediente = ing.tblNombreIngrediente,
                    Cantidad = ing.tblCantidad,
                    PrecioPaquete = ing.tblPrecioPaquete,
                    PrecioUnitario = ing.tblPrecioUnitario
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ingrediente: Ref {Ref}", referencia);
                throw;
            }
        }

        public async Task<bool> ExisteDuplicado(int referencia, string nombre, Guid excluirId)
        {
            try
            {
                _logger.LogInformation("Verificando duplicado de ingrediente: Ref {Ref}, Nombre {Nombre}", referencia, nombre);

                var existe = await _dbContext.tblIngredientes.AnyAsync(i =>
                    i.tblId != excluirId &&
                    (i.tblReferencia == referencia || i.tblNombreIngrediente.ToLower() == nombre.ToLower()));

                if (existe)
                {
                    _logger.LogWarning("Ingrediente duplicado detectado: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
                }

                return existe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar duplicado de ingrediente: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
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
                _logger.LogInformation("Modificando ingrediente: {@Ingrediente}", new { ingredienteModificador.Ref, ingredienteModificador.NameIngredient });

                ArgumentNullException.ThrowIfNull(ingredienteModificador);

                var ingredienteEnDb = await _dbContext.tblIngredientes.FirstOrDefaultAsync(i => i.tblReferencia == ingredienteModificador.Ref);

                if (ingredienteEnDb == null)
                {
                    _logger.LogWarning("Ingrediente no encontrado para modificar: Ref {Ref}", ingredienteModificador.Ref);
                    return null;
                }

                ingredienteEnDb.tblNombreIngrediente = ingredienteModificador.NameIngredient;
                ingredienteEnDb.tblCantidad = ingredienteModificador.Quantity;
                ingredienteEnDb.tblPrecioPaquete = ingredienteModificador.PrecioPack;
                ingredienteEnDb.tblPrecioUnitario = ingredienteModificador.PrecioUnidad;

                _dbContext.tblIngredientes.Update(ingredienteEnDb);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Ingrediente modificado exitosamente: {@IngredienteModificado}", new { ingredienteEnDb.tblReferencia, ingredienteEnDb.tblNombreIngrediente });

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
                _logger.LogError(ex, "Error al modificar ingrediente: {@Ingrediente}", new { ingredienteModificador.Ref, ingredienteModificador.NameIngredient });
                throw;
            }
        }
    }
}
