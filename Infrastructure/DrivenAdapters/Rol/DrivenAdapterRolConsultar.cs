using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    /// <summary>
    /// Adaptador conducido para consultar roles de la base de datos
    /// </summary>
    public class DrivenAdapterRolConsultar : PortDrivenRolConsultar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterRolConsultar> _logger;

        public DrivenAdapterRolConsultar(ApplicationDbContext dbContext, ILogger<DrivenAdapterRolConsultar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Consulta un rol por nombre en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre del rol a consultar</param>
        /// <returns>El rol encontrado o null si no existe</returns>
        public async Task<Domain.Entities.Rol> ConsultarRolAsync(string nombre)
        {
            try
            {
                _logger.LogInformation("Consultando rol por nombre: {Nombre}", nombre);

                var tblRol = await _dbContext.tblRoles
                    .Where(r => r.tblNombre.ToLower().Contains(nombre.ToLower()))
                    .FirstOrDefaultAsync();

                if (tblRol == null)
                {
                    _logger.LogWarning("Rol no encontrado: {Nombre}", nombre);
                    return null;
                }

                _logger.LogInformation("Rol encontrado exitosamente: {Nombre}", nombre);
                return new Domain.Entities.Rol
                { 
                    Identificacion = tblRol.tblIdentificacion,
                    Tipo = tblRol.tblTipo,
                    Nombre = tblRol.tblNombre
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar rol: {Nombre}", nombre);
                throw;
            }
        }
    }
}