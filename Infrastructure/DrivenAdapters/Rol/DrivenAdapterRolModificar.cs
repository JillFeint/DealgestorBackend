using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    /// <summary>
    /// Adaptador conducido para modificar roles en la base de datos
    /// </summary>
    public class DrivenAdapterRolModificar : PortDrivenRolModificar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterRolModificar> _logger;

        public DrivenAdapterRolModificar(ApplicationDbContext dbContext, ILogger<DrivenAdapterRolModificar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
       
        /// <summary>
        /// Obtiene un rol por su nombre y tipo
        /// </summary>
        /// <param name="name">Nombre del rol</param>
        /// <param name="tipe">Tipo del rol</param>
        /// <returns>Entidad Rol encontrada o null si no existe</returns>
        public async Task<Domain.Entities.Rol> ObtenerRolNombreTipo(string name, string tipe)
        {
            try
            {
                _logger.LogInformation("Obteniendo rol por nombre y tipo: {Name}, {Type}", name, tipe);

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(tipe))
                {
                    _logger.LogWarning("Parámetros de búsqueda vacíos: {Name}, {Type}", name, tipe);
                    return null;
                }

                var rolEncontrado = await _dbContext.tblRoles
                    .FirstOrDefaultAsync(r => r.tblNombre.ToLower() == name.ToLower() && r.tblTipo.ToLower() == tipe.ToLower());

                if (rolEncontrado == null)
                {
                    _logger.LogWarning("Rol no encontrado: {Name}, {Type}", name, tipe);
                    return null;
                }

                _logger.LogInformation("Rol encontrado: {Name}, {Type}", name, tipe);
                return new Domain.Entities.Rol
                {
                    Identificacion = rolEncontrado.tblIdentificacion,
                    Tipo = rolEncontrado.tblTipo,
                    Nombre = rolEncontrado.tblNombre
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol: {Name}, {Type}", name, tipe);
                throw;
            }
        }

        /// <summary>
        /// Modifica un rol existente en la base de datos
        /// </summary>
        /// <param name="rolAModificar">Entidad del rol a modificar</param>
        /// <returns>Rol modificado o null si no existe</returns>
        public async Task<Domain.Entities.Rol> ModificarRol(Domain.Entities.Rol rolAModificar)
        {
            try
            {
                _logger.LogInformation("Modificando rol: {@Rol}", new { rolAModificar.Nombre, rolAModificar.Tipo });

                ArgumentNullException.ThrowIfNull(rolAModificar);

                var rolEnDb = await _dbContext.tblRoles.FindAsync(rolAModificar.Identificacion);

                if (rolEnDb == null)
                {
                    _logger.LogWarning("Rol no encontrado para modificar: {Identificacion}", rolAModificar.Identificacion);
                    return null;
                }

                rolEnDb.tblNombre = rolAModificar.Nombre;
                rolEnDb.tblTipo = rolAModificar.Tipo;

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Rol modificado exitosamente: {@RolModificado}", new { rolEnDb.tblNombre, rolEnDb.tblTipo });

                return new Domain.Entities.Rol
                {
                    Identificacion = rolEnDb.tblIdentificacion,
                    Tipo = rolEnDb.tblTipo,
                    Nombre = rolEnDb.tblNombre
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar rol: {@Rol}", new { rolAModificar.Nombre, rolAModificar.Tipo });
                throw;
            }
        }
    }
}
