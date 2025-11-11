using Application.Ports.DrivenPorts.Rol;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    public class DrivenAdapterRolEliminar : PortDrivenRolEliminar
    {
        private readonly ApplicationDbContext _dbContext;

        public DrivenAdapterRolEliminar(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> EliminarRol(string nombre)
        {
            var rol = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre);

            if (rol == null)
            {
                return false;
            }

            _dbContext.Roles.Remove(rol);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}