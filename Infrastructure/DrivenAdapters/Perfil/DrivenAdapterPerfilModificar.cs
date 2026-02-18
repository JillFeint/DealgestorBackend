using Application.DTOs.Perfiles;
using Application.Ports.DrivenPorts.Perfil;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Perfil
{
    /// <summary>
    /// Adaptador conducido para modificar perfiles en la base de datos
    /// </summary>
    public class DrivenAdapterPerfilModificar : PortDrivenPerfilModificar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterPerfilModificar> _logger;

        public DrivenAdapterPerfilModificar(ApplicationDbContext dbContext, ILogger<DrivenAdapterPerfilModificar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene un perfil por su email
        /// </summary>
        /// <param name="email">Email del perfil a obtener</param>
        /// <returns>DTO del perfil encontrado o null si no existe</returns>
        public async Task<PerfilDTODriven?> ObtenerPerfilPorEmail(string email)
        {
            try
            {
                _logger.LogInformation("Obteniendo perfil por email: {Email}", email);

                var perfil = await _dbContext.tblPerfiles
                    .FirstOrDefaultAsync(p => p.tblEmail == email);

                if (perfil == null)
                {
                    _logger.LogWarning("Perfil no encontrado: {Email}", email);
                    return null;
                }

                _logger.LogInformation("Perfil encontrado: {Email}", email);
                return perfil;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil: {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Modifica un perfil existente en la base de datos
        /// </summary>
        /// <param name="perfil">DTO del perfil a modificar</param>
        /// <returns>Perfil modificado</returns>
        public async Task<PerfilDTODriven> ModificarPerfil(PerfilDTODriven perfil)
        {
            try
            {
                _logger.LogInformation("Modificando perfil: {@Perfil}", new { perfil.tblEmail });

                _dbContext.tblPerfiles.Update(perfil);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Perfil modificado exitosamente: {@PerfilModificado}", new { perfil.tblEmail });
                return perfil;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar perfil: {@Perfil}", new { perfil.tblEmail });
                throw;
            }
        }
    }
}
