using Application.DTOs.Ingredientes;
using Application.Ports.DrivenPorts.Ingrediente;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Ingrediente
{
    public class DrivenAdapterIngredienteCrear : PortDrivenIngredienteCrear
    {
        private readonly ApplicationDbContext _Context;

        public DrivenAdapterIngredienteCrear(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        public async Task<bool> ExisteIngredienteNombre(string nombre, string tipo)
        {
            return await _Context.tblRoles.AnyAsync(i => 
                i.tblNombre.ToLower() == nombre.ToLower() && 
                i.tblTipo.ToLower() == tipo.ToLower());
        }

        public async Task<Domain.Entities.Ingrediente> CrearIngrediente(IngredienteDTODriver ingrediente)
        {
            if (ingrediente.Id == Guid.Empty)
                ingrediente.Id = Guid.NewGuid();

                var nuevoIngrediente = new IngredienteDTODriven(
                tblId: ingrediente.Id,
                tblReferencia: ingrediente.Referencia,
                tblNombreIngrediente: ingrediente.NombreIngrediente,
                tblPrecioUnitario: ingrediente.PrecioUnitario
            );

            _Context.tblIngredientes.Add(nuevoIngrediente);
            await _Context.SaveChangesAsync();

            return new Domain.Entities.Ingrediente
            {
                Id = nuevoIngrediente.tblId,
                Referencia = nuevoIngrediente.tblReferencia,
                NombreIngrediente = nuevoIngrediente.tblNombreIngrediente,
                PrecioUnitario = nuevoIngrediente.tblPrecioUnitario
            };
        }
    }
}