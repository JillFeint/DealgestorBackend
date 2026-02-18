using Application.DTOs.Perfiles;
using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Perfil;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Perfil
{
    /// <summary>
    /// Adaptador conducido para crear perfiles en la base de datos
    /// </summary>
    public class DrivenAdapterPerfilCrear : PortDrivenPerfilCrear
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterPerfilCrear> _logger;

        public DrivenAdapterPerfilCrear(ApplicationDbContext dbContext, ILogger<DrivenAdapterPerfilCrear> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Verifica si existe un perfil con el email especificado
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <returns>True si existe, False en caso contrario</returns>
        public async Task<bool> ExistePerfilPorEmail(string email)
        {
            try
            {
                _logger.LogInformation("Verificando existencia de perfil: {Email}", email);

                var existe = await _dbContext.tblPerfiles.AnyAsync(p => 
                    p.tblEmail.ToLower() == email.ToLower());

                if (existe)
                {
                    _logger.LogWarning("Perfil ya existe: {Email}", email);
                }
                else
                {
                    _logger.LogInformation("Perfil no existe: {Email}", email);
                }

                return existe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia de perfil: {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo perfil en la base de datos
        /// </summary>
        /// <param name="perfil">DTO del perfil a crear</param>
        /// <returns>Entidad Perfil creada</returns>
        public async Task<Domain.Entities.Perfil> CrearPerfil(PerfilIngresoDTODriver perfil)
        {
            try
            {
                _logger.LogInformation("Creando nuevo perfil: {@Perfil}", new { perfil.Email });

                // Buscar o crear el rol por defecto "normalUserRolPermise"
                var rolPorDefecto = await _dbContext.tblRoles.FirstOrDefaultAsync(rol => 
                    rol.tblNombre == "NormalUserRolPermise");

                if (rolPorDefecto == null)
                {
                    _logger.LogInformation("Rol por defecto no existe, creando: NormalUserRolPermise");

                    rolPorDefecto = new RolDTODriven
                    {
                        tblIdentificacion = Guid.NewGuid(),
                        tblNombre = "NormalUserRolPermise",
                        tblTipo = "NormalFreeUser"
                    };
                    _dbContext.tblRoles.Add(rolPorDefecto);
                    await _dbContext.SaveChangesAsync();

                    _logger.LogInformation("Rol por defecto creado: {RolNombre}", rolPorDefecto.tblNombre);
                }

                var nuevoPerfilDriven = new PerfilDTODriven(
                    tblIdentidad: Guid.NewGuid(),
                    tblEmail: perfil.Email,
                    tblCodigoSecreto: perfil.CodigoSecreto,
                    tblFechaCreacion: DateTime.UtcNow,
                    tblNegocios: new List<Domain.Entities.Negocio>(),
                    tblPermisosRolIds: new List<Guid> { rolPorDefecto.tblIdentificacion }                   
                );

                _dbContext.tblPerfiles.Add(nuevoPerfilDriven);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Perfil creado exitosamente: {@PerfilCreado}", new { nuevoPerfilDriven.tblEmail, nuevoPerfilDriven.tblFechaCreacion });

                return new Domain.Entities.Perfil
                {
                    Email = nuevoPerfilDriven.tblEmail,
                    FechaCreacion = nuevoPerfilDriven.tblFechaCreacion
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear perfil: {@Perfil}", new { perfil.Email });
                throw;
            }
        }
    }
}