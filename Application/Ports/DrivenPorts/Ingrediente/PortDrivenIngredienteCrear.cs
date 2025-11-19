using Application.DTOs.Ingredientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Ingrediente
{
    public interface PortDrivenIngredienteCrear
    {
        Task<bool> ExisteIngredienteNombre(int referencia, string nombre);
        Task<Domain.Entities.Ingrediente> CrearIngrediente(IngredienteDTODriver);
    }
}
