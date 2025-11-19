using Application.DTOs.Ingredientes;
using Application.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Rol
{
    public interface PortDriverIngredienteCrear
    {
        Task<IngredienteDTODriver> CrearIngrediente(IngredienteDTODriver nuevoIngredienteDTO);
    }
}
