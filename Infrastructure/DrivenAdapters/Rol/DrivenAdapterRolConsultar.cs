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

        public async Task<RolDTODriver> ConsultarRolAsync(string nombre)
        {
            var rol = await _dbContext.Roles
                .Where(r => r.Nombre.ToLower().Contains(nombre.ToLower()))
                .FirstOrDefaultAsync();

            if (rol == null)
            {
                return null;
            }

            return new RolDTODriver
            {
                Identificacion = rol.tblIdentificacion,
                Tipo = rol.tblTipo,
                Nombre = rol.tblNombre
            };
        }
    }
}