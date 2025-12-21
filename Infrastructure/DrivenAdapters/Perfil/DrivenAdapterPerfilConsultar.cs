using Application.Ports.DrivenPorts.Perfil;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Perfil
{
    /// <summary>
    /// Adaptador conducido para consultar perfiles de la base de datos
    /// </summary>
    public class DrivenAdapterPerfilConsultar : PortDrivenPerfilConsultar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterPerfilConsultar> _logger;

        public DrivenAdapterPerfilConsultar(ApplicationDbContext dbContext, ILogger<DrivenAdapterPerfilConsultar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Consulta un perfil por email en la base de datos
        /// </summary>
        /// <param name="email">Email del perfil a consultar</param>
        /// <returns>El perfil encontrado o null si no existe</returns>
        public async Task<Domain.Entities.Perfil> ConsultarPerfilPorEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation("Consultando perfil por email: {Email}", email);

                var tblPerfil = await _dbContext.tblPerfiles
                    .FirstOrDefaultAsync(p => p.tblEmail.ToLower() == email.ToLower());

                if (tblPerfil == null)
                {
                    _logger.LogWarning("Perfil no encontrado: {Email}", email);
                    return null;
                }

                _logger.LogInformation("Perfil encontrado exitosamente: {Email}", email);
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
                _logger.LogError(ex, "Error al consultar perfil: {Email}", email);
                throw;
            }
        }
    }
}