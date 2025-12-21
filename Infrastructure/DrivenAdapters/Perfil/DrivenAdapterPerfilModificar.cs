using Application.DTOs.Perfiles;
using Application.Ports.DrivenPorts.Perfil;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Perfil
{
    public class DrivenAdapterPerfilModificar : PortDrivenPerfilModificar
    {
        private readonly ApplicationDbContext _dbContext;

        public DrivenAdapterPerfilModificar(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<PerfilDTODriven?> ObtenerPerfilPorEmail(string email)
        {
            return await _dbContext.tblPerfiles
                .FirstOrDefaultAsync(p => p.tblEmail == email);
        }

        public async Task<PerfilDTODriven> ModificarPerfil(PerfilDTODriven perfil)
        {
            _dbContext.tblPerfiles.Update(perfil);
            await _dbContext.SaveChangesAsync();
            return perfil;
        }
    }
}
