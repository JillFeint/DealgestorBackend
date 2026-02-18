using Application.Ports.DrivenPorts.Perfil;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Perfil
{
    /// <summary>
    /// Adaptador conducido para eliminar perfiles de la base de datos
    /// </summary>
    public class DrivenAdapterPerfilEliminar : PortDrivenPerfilEliminar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterPerfilEliminar> _logger;

        public DrivenAdapterPerfilEliminar(ApplicationDbContext dbContext, ILogger<DrivenAdapterPerfilEliminar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene un perfil por su email
        /// </summary>
        /// <param name="email">Email del perfil a obtener</param>
        /// <returns>Entidad Perfil encontrada o null si no existe</returns>
        public async Task<Domain.Entities.Perfil> ObtenerPerfilPorEmail(string email)
        {
            try
            {
                _logger.LogInformation("Obteniendo perfil por email: {Email}", email);

                var tblPerfil = await _dbContext.tblPerfiles
                    .FirstOrDefaultAsync(p => p.tblEmail.ToLower() == email.ToLower());

                if (tblPerfil == null)
                {
                    _logger.LogWarning("Perfil no encontrado: {Email}", email);
                    return null;
                }

                _logger.LogInformation("Perfil encontrado: {Email}", email);
                return new Domain.Entities.Perfil
                {
                    Identidad = tblPerfil.tblIdentidad,
                    Email = tblPerfil.tblEmail,
                    CodigoSecreto = tblPerfil.tblCodigoSecreto,
                    FechaCreacion = tblPerfil.tblFechaCreacion
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil: {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Elimina un perfil por su email
        /// </summary>
        /// <param name="email">Email del perfil a eliminar</param>
        /// <returns>True si se eliminó correctamente, False si el perfil no existe</returns>
        public async Task<bool> EliminarPerfilPorEmail(string email)
        {
            try
            {
                _logger.LogInformation("Eliminando perfil: {Email}", email);

                var tblPerfil = await _dbContext.tblPerfiles
                    .FirstOrDefaultAsync(p => p.tblEmail.ToLower() == email.ToLower());

                if (tblPerfil == null)
                {
                    _logger.LogWarning("Intento de eliminar perfil inexistente: {Email}", email);
                    return false;
                }

                _dbContext.tblPerfiles.Remove(tblPerfil);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Perfil eliminado exitosamente: {Email}", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar perfil: {Email}", email);
                throw;
            }
        }
    }
}
