using Application.Ports.DrivenPorts.Rol;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    public class DrivenAdapterRolEliminar : PortDrivenIngredienteEliminar
    {
        private readonly ApplicationDbContext _dbContext;

        public DrivenAdapterRolEliminar(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> EliminarRol(string nombre)
        {
            var rol = await _dbContext.tblRoles.FirstOrDefaultAsync(r => r.tblNombre == nombre);

            if (rol == null)
            {
                return false;
            }

            _dbContext.tblRoles.Remove(rol);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}