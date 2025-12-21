using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    /// <summary>
    /// Adaptador conducido para crear roles en la base de datos
    /// </summary>
    public class DrivenAdapterRolCrear : PortDrivenRolCrear
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterRolCrear> _logger;

        public DrivenAdapterRolCrear(ApplicationDbContext dbContext, ILogger<DrivenAdapterRolCrear> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Verifica si existe un rol con el nombre y tipo especificado
        /// </summary>
        /// <param name="nombre">Nombre del rol a verificar</param>
        /// <param name="tipo">Tipo del rol a verificar</param>
        /// <returns>True si existe, False en caso contrario</returns>
        public async Task<bool> ExisteRolPorNombre(string nombre, string tipo)
        {
            try
            {
                _logger.LogInformation("Verificando existencia de rol: {Nombre}, {Tipo}", nombre, tipo);

                var existe = await _dbContext.tblRoles.AnyAsync(r => 
                    r.tblNombre.ToLower() == nombre.ToLower() && 
                    r.tblTipo.ToLower() == tipo.ToLower());

                if (existe)
                {
                    _logger.LogWarning("Rol ya existe: {Nombre}, {Tipo}", nombre, tipo);
                }
                else
                {
                    _logger.LogInformation("Rol no existe: {Nombre}, {Tipo}", nombre, tipo);
                }

                return existe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia de rol: {Nombre}, {Tipo}", nombre, tipo);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo rol en la base de datos
        /// </summary>
        /// <param name="rol">DTO del rol a crear</param>
        /// <returns>Entidad Rol creada</returns>
        public async Task<Domain.Entities.Rol> CrearRol(RolDTODriver rol)
        {
            try
            {
                _logger.LogInformation("Creando nuevo rol: {@Rol}", new { rol.Name, rol.Tipe });

                if (rol.Identidad == Guid.Empty)
                    rol.Identidad = Guid.NewGuid();

                var nuevoRol = new RolDTODriven(
                    tblIdentificacion: rol.Identidad,
                    tblTipo: rol.Tipe,
                    tblNombre: rol.Name
                );

                _dbContext.tblRoles.Add(nuevoRol);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Rol creado exitosamente: {@RolCreado}", new { nuevoRol.tblIdentificacion, nuevoRol.tblNombre });

                return new Domain.Entities.Rol
                {
                    Identificacion = nuevoRol.tblIdentificacion,
                    Tipo = nuevoRol.tblTipo,
                    Nombre = nuevoRol.tblNombre
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear rol: {@Rol}", new { rol.Name, rol.Tipe });
                throw;
            }
        }
    }
}