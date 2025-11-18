using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    public class DrivenAdapterRolModificar : PortDrivenIngredienteModificar
    {
        private readonly ApplicationDbContext _dbContext;

        public DrivenAdapterRolModificar(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
       
        public async Task<Domain.Entities.Rol> ObtenerRolPorNombreTipo(string name, string tipe)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(tipe))
                return null;

            var rolEncontrado = await _dbContext.tblRoles
                .FirstOrDefaultAsync(r => r.tblNombre == name && r.tblTipo == tipe);

            if (rolEncontrado == null) return null;

            // Mapear de la entidad de base de datos a la entidad de dominio
            return new Domain.Entities.Rol
            {
                Identificacion = rolEncontrado.tblIdentificacion,
                Tipo = rolEncontrado.tblTipo,
                Nombre = rolEncontrado.tblNombre
            };
        }

        public async Task<Domain.Entities.Rol> ModificarRol(Domain.Entities.Rol rolAModificar)
        {
            ArgumentNullException.ThrowIfNull(rolAModificar);

            var rolEnDb = await _dbContext.tblRoles.FindAsync(rolAModificar.Identificacion);

            if (rolEnDb == null)
            {
                return null; 
            }

            rolEnDb.tblNombre = rolAModificar.Nombre;
            rolEnDb.tblTipo = rolAModificar.Tipo;

            await _dbContext.SaveChangesAsync();

            return new Domain.Entities.Rol
            {
                Identificacion = rolEnDb.tblIdentificacion,
                Tipo = rolEnDb.tblTipo,
                Nombre = rolEnDb.tblNombre
            };
        }
    }
}
