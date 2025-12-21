using Application.DTOs.Perfiles;
using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Perfil;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Perfil
{
    public class DrivenAdapterPerfilCrear : PortDrivenPerfilCrear
    {
        private readonly ApplicationDbContext _context;

        public DrivenAdapterPerfilCrear(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistePerfilPorEmail(string email)
        {
            return await _context.tblPerfiles.AnyAsync(p => 
                p.tblEmail.ToLower() == email.ToLower());
        }

        public async Task<Domain.Entities.Perfil> CrearPerfil(PerfilIngresoDTODriver perfil)
        {
            // Buscar o crear el rol por defecto "normalUserRolPermise"
            var rolPorDefecto = await _context.tblRoles.FirstOrDefaultAsync(rol => 
                rol.tblNombre == "NormalUserRolPermise");

            if (rolPorDefecto == null)
            {
                rolPorDefecto = new RolDTODriven
                {
                    tblIdentificacion = Guid.NewGuid(),
                    tblNombre = "NormalUserRolPermise",
                    tblTipo = "NormalFreeUser"
                };
                _context.tblRoles.Add(rolPorDefecto);
                await _context.SaveChangesAsync();
            }

            var nuevoPerfilDriven = new PerfilDTODriven(
                tblIdentidad: Guid.NewGuid(),
                tblEmail: perfil.Email,
                tblCodigoSecreto: perfil.CodigoSecreto,
                tblFechaCreacion: DateTime.UtcNow,
                tblNegocios: new List<Domain.Entities.Negocio>(),
                tblPermisosRolIds: new List<Guid> { rolPorDefecto.tblIdentificacion }                   
            );

            _context.tblPerfiles.Add(nuevoPerfilDriven);
            await _context.SaveChangesAsync();

            return new Domain.Entities.Perfil
            {
                Email = nuevoPerfilDriven.tblEmail,
                FechaCreacion = nuevoPerfilDriven.tblFechaCreacion
            };
        }
    }
}