using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    public class DrivenAdapterRolCrear : PortDrivenRolCrear
    {
        private readonly ApplicationDbContext _dbContext;

        public DrivenAdapterRolCrear(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExisteRolPorNombre(string nombre, string tipo)
        {
            // Comprueba si ya existe un rol con el mismo nombre y tipo, ignorando mayúsculas/minúsculas.
            return await _dbContext.Roles.AnyAsync(r => 
                r.Nombre.ToLower() == nombre.ToLower() && 
                r.Tipo.ToLower() == tipo.ToLower());
        }

        public async Task<RolDTODriven> CrearRol(RolDTODriver rol)
        {
            var nuevoRol = new Domain.Entities.Rol(
                Identificacion: rol.tblIdentificacion,
                Tipo: rol.tblTipo,
                Nombre: rol.tblNombre
            );

            _dbContext.Roles.Add(nuevoRol);
            await _dbContext.SaveChangesAsync();

            // Devuelve el DTO con los datos del rol recién creado.
            return new RolDTODriven
            {
                tblIdentificacion = nuevoRol.Identificacion,
                tblTipo = nuevoRol.Tipo,
                tblNombre = nuevoRol.Nombre
            };
        }
    }
}