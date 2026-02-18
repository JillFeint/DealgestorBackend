using Application.Ports.DrivenPorts.Ingrediente;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Ingrediente
{
    /// <summary>
    /// Adaptador conducido para consultar ingredientes de la base de datos
    /// </summary>
    public class DrivenAdapterIngredienteConsultar : PortDrivenIngredienteConsultar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterIngredienteConsultar> _logger;

        public DrivenAdapterIngredienteConsultar(ApplicationDbContext dbContext, ILogger<DrivenAdapterIngredienteConsultar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene un ingrediente por su nombre
        /// </summary>
        /// <param name="nombreIngrediente">Nombre del ingrediente a consultar</param>
        /// <returns>Entidad Ingrediente encontrada o null si no existe</returns>
        public async Task<Domain.Entities.Ingrediente> ObtenerNombre(string nombreIngrediente)
        {
            try
            {
                _logger.LogInformation("Consultando ingrediente por nombre: {Nombre}", nombreIngrediente);

                var tblIngrediente = await _dbContext.tblIngredientes
                    .Where(i => i.tblNombreIngrediente.ToLower().Contains(nombreIngrediente.ToLower()))
                    .FirstOrDefaultAsync();

                if (tblIngrediente == null)
                {
                    _logger.LogWarning("Ingrediente no encontrado: {Nombre}", nombreIngrediente);
                    return null;
                }

                _logger.LogInformation("Ingrediente encontrado exitosamente: {Nombre}", nombreIngrediente);
                return new Domain.Entities.Ingrediente
                {
                    Id = tblIngrediente.tblId,
                    Referencia = tblIngrediente.tblReferencia,
                    NombreIngrediente = tblIngrediente.tblNombreIngrediente,
                    Cantidad = tblIngrediente.tblCantidad,
                    PrecioPaquete = tblIngrediente.tblPrecioPaquete,
                    PrecioUnitario = tblIngrediente.tblPrecioUnitario  
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar ingrediente: {Nombre}", nombreIngrediente);
                throw;
            }
        }
    }
}