using Application.Ports.DrivenPorts.Rol;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    public class DrivenAdapterRolModificar : IPortDrivenRolModificar
    {
        private readonly ApplicationDbContext _dbContext;

        public DrivenAdapterRolModificar(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Entities.Rol> ModificarRol(Domain.Entities.Rol rol)
        {
            _dbContext.Roles.Update(rol);
            await _dbContext.SaveChangesAsync();
            return rol;
        }
    }
}
