using Application.DTOs.Ingredientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DriverPorts.Ingrediente
{
    public interface PortDriverIngredienteCrear
    {
        Task<IngredienteDTODriver> CrearNuevoIngrediente(IngredienteDTODriver nuevoIngredienteDTO);
    }
}
