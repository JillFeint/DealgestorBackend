using Application.Ports.DrivenPorts.Perfil;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Perfil
{
    public class DrivenAdapterPerfilConsultar : PortDrivenPerfilConsultar
    {
        private readonly ApplicationDbContext _context;

        public DrivenAdapterPerfilConsultar(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Domain.Entities.Perfil> ConsultarPerfilPorEmailAsync(string email)
        {
            var tblPerfil = await _context.tblPerfiles
                .FirstOrDefaultAsync(p => p.tblEmail.ToLower() == email.ToLower());

            if (tblPerfil == null)
            {
                return null;
            }

            return new Domain.Entities.Perfil
            {
                Identidad = tblPerfil.tblIdentidad,
                Email = tblPerfil.tblEmail,
                CodigoSecreto = tblPerfil.tblCodigoSecreto,
                FechaCreacion = tblPerfil.tblFechaCreacion
            };
        }
    }
}