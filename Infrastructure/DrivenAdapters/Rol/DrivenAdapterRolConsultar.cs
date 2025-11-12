using Application.Ports.DrivenPorts.Rol;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Roles;

namespace Infrastructure.DrivenAdapters.Rol
{
    public class DrivenAdapterRolConsultar : PortDrivenRolConsultar
    {
        private readonly ApplicationDbContext _dbContext;

        public DrivenAdapterRolConsultar(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Entities.Rol> ConsultarRolAsync(string nombre)
        {
            var tblRol = await _dbContext.tblRoles
                .Where(r => r.Nombre.ToLower().Contains(nombre.ToLower()))
                .FirstOrDefaultAsync();

            if (tblRol == null)
            {
                return null;
            }

            return new Domain.Entities.Rol
            (
                Identificacion =  tblRol.tblIdentificacion,
                Tipo = tblRol.tblTipo,
                Nombre = tblRol.tblNombre
            );
        }
    }
}