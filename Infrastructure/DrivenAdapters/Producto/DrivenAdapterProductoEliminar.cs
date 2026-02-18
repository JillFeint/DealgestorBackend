using Application.Ports.DrivenPorts.Producto;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Producto
{
    /// <summary>
    /// Adaptador conducido para eliminar productos de la base de datos
    /// </summary>
    public class DrivenAdapterProductoEliminar : PortDrivenProductoEliminar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterProductoEliminar> _logger;

        public DrivenAdapterProductoEliminar(ApplicationDbContext dbContext, ILogger<DrivenAdapterProductoEliminar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Elimina un producto por su referencia
        /// </summary>
        public async Task<bool> EliminarProducto(int referencia)
        {
            try
            {
                _logger.LogInformation("Eliminando producto con referencia: {Ref}", referencia);

                var producto = await _dbContext.tblProductos.FirstOrDefaultAsync(p => p.tblReferencia == referencia);

                if (producto == null)
                {
                    _logger.LogWarning("Intento de eliminar producto inexistente: Ref {Ref}", referencia);
                    return false;
                }

                _dbContext.tblProductos.Remove(producto);
                int result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Producto eliminado exitosamente: Ref {Ref}", referencia);
                    return true;
                }

                _logger.LogWarning("Fallo al eliminar producto: Ref {Ref}", referencia);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto: Ref {Ref}", referencia);
                throw;
            }
        }
    }
}