using Application.DTOs.Ingredientes;
using Application.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Ingrediente
{
    public interface PortDrivenIngredienteCrear
    {
        Task<bool> ExisteIngredienteNombre(string nombre, string tipo);
        Task<Domain.Entities.Ingrediente> CrearIngrediente(IngredienteDTODriver);
    }
}
