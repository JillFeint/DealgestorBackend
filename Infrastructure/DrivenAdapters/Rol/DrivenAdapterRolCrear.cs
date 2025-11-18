using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Rol
{
    public class DrivenAdapterRolCrear : PortDrivenIngredienteCrear
    {
        private readonly ApplicationDbContext _dbContext;

        public DrivenAdapterRolCrear(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExisteRolPorNombre(string nombre, string tipo)
        {
            return await _dbContext.tblRoles.AnyAsync(r => 
                r.tblNombre.ToLower() == nombre.ToLower() && 
                r.tblTipo.ToLower() == tipo.ToLower());
        }

        public async Task<Domain.Entities.Rol> CrearRol(RolDTODriver rol)
        {
            rol.Identidad = Guid.NewGuid();

            var nuevoRol = new RolDTODriven(
                tblIdentificacion: rol.Identidad,
                tblTipo: rol.Tipe,
                tblNombre: rol.Name
            );

            _dbContext.tblRoles.Add(nuevoRol);
            await _dbContext.SaveChangesAsync();

            // Devuelve el DTO con los datos del rol recién creado.
            return new Domain.Entities.Rol
            {
                Identificacion = nuevoRol.tblIdentificacion,
                Tipo = nuevoRol.tblTipo,
                Nombre = nuevoRol.tblNombre
            };
        }
    }
}