using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    public class DrivenAdapterRolConsultar : PortDrivenRolConsultar
    {
        private readonly ApplicationDbContext _context;

        public DrivenAdapterRolConsultar(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Domain.Entities.Rol> ConsultarRolAsync(string nombre)
        {
            var tblRol = await _context.tblRoles
                .Where(r => r.tblNombre.ToLower().Contains(nombre.ToLower()))
                .FirstOrDefaultAsync();

            if (tblRol == null)
            {
                return null;
            }

            return new Domain.Entities.Rol
            { 
                Identificacion =  tblRol.tblIdentificacion,
                Tipo = tblRol.tblTipo,
                Nombre = tblRol.tblNombre
            };
        }
    }
}