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

        public async Task<bool> ExisteIngredienteNombre(int referencia, string nombre)
        {
            return await _Context.tblIngredientes.AnyAsync(i => 
                i.tblReferencia == referencia && 
                i.tblNombreIngrediente.ToLower() == nombre.ToLower());
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