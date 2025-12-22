using Application.Ports.DrivenPorts.Rol;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    /// <summary>
    /// Adaptador conducido para eliminar roles de la base de datos
    /// </summary>
    public class DrivenAdapterRolEliminar : PortDrivenRolEliminar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterRolEliminar> _logger;

        public DrivenAdapterRolEliminar(ApplicationDbContext dbContext, ILogger<DrivenAdapterRolEliminar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Domain.Entities.Rol?> ObtenerRolPorNombre(string nombreNormalizado)
        {
            try
            {
                _logger.LogInformation("Buscando rol por nombre (normalizado): {Nombre}", nombreNormalizado);

                var rol = await _dbContext.tblRoles
                    .FirstOrDefaultAsync(r => r.tblNombre.ToLower() == nombreNormalizado.ToLower());

                if (rol == null)
                {
                    _logger.LogWarning("Rol no encontrado: {Nombre}", nombreNormalizado);
                    return null;
                }

                return new Domain.Entities.Rol
                {
                    Identificacion = rol.tblIdentificacion,
                    Nombre = rol.tblNombre,
                    Tipo = rol.tblTipo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol: {Nombre}", nombreNormalizado);
                throw;
            }
        }

        public async Task<bool> EstaRolEnUso(Guid rolId)
        {
            try
            {
                _logger.LogInformation("Verificando si el rol está en uso: {RolId}", rolId);

                bool enUso = await _dbContext.tblPerfiles
                    .AnyAsync(p => p.tblPermisosRolIds != null && p.tblPermisosRolIds.Contains(rolId));

                if (enUso)
                {
                    _logger.LogWarning("Rol en uso, no se puede eliminar: {RolId}", rolId);
                }

                return enUso;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar uso de rol: {RolId}", rolId);
                throw;
            }
        }

        /// <summary>
        /// Elimina un rol de la base de datos por su nombre
        /// </summary>
        /// <param name="nombre">Nombre del rol a eliminar</param>
        /// <returns>True si se eliminó correctamente, False si el rol no existe</returns>
        public async Task<bool> EliminarRol(string nombre)
        {
            try
            {
                _logger.LogInformation("Eliminando rol: {Nombre}", nombre);

                var rol = await _dbContext.tblRoles.FirstOrDefaultAsync(r => r.tblNombre.ToLower() == nombre.ToLower());

                if (rol == null)
                {
                    _logger.LogWarning("Intento de eliminar rol inexistente: {Nombre}", nombre);
                    return false;
                }

                _dbContext.tblRoles.Remove(rol);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Rol eliminado exitosamente: {Nombre}", nombre);
                    return true;
                }

                _logger.LogWarning("Fallo al eliminar rol: {Nombre}", nombre);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar rol: {Nombre}", nombre);
                throw;
            }
        }
    }
}