using Application.Ports.DrivenPorts.Ingrediente;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Ingrediente
{
    /// <summary>
    /// Adaptador conducido para eliminar ingredientes de la base de datos
    /// </summary>
    public class DrivenAdapterIngredienteEliminar : PortDrivenIngredienteEliminar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterIngredienteEliminar> _logger;

        public DrivenAdapterIngredienteEliminar(ApplicationDbContext dbContext, ILogger<DrivenAdapterIngredienteEliminar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Elimina un ingrediente por su referencia
        /// </summary>
        /// <param name="referencia">Referencia del ingrediente a eliminar</param>
        /// <returns>True si se eliminó correctamente, False si el ingrediente no existe</returns>
        public async Task<bool> EliminarIngrediente(int referencia)
        {
            try
            {
                _logger.LogInformation("Eliminando ingrediente con referencia: {Ref}", referencia);

                var ingrediente = await _dbContext.tblIngredientes.FirstOrDefaultAsync(r => r.tblReferencia == referencia);

                if (ingrediente == null)
                {
                    _logger.LogWarning("Intento de eliminar ingrediente inexistente: Ref {Ref}", referencia);
                    return false;
                }

                _dbContext.tblIngredientes.Remove(ingrediente);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Ingrediente eliminado exitosamente: Ref {Ref}", referencia);
                    return true;
                }

                _logger.LogWarning("Fallo al eliminar ingrediente: Ref {Ref}", referencia);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar ingrediente: Ref {Ref}", referencia);
                throw;
            }
        }
    }
}