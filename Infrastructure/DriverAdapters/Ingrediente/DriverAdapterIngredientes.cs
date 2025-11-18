using Application.DTOs.Ingredientes;
using Application.Ports.DriverPorts.Ingrediente;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DriverAdapters.Ingrediente
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverAdapterIngredientes : ControllerBase
    {
        private readonly PortDriverIngredienteConsultar _ingredientePort;

        public DriverAdapterIngredientes(PortDriverIngredienteConsultar ingredientePort)
        {
            _ingredientePort = ingredientePort ?? throw new ArgumentNullException(nameof(ingredientePort));
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> ConsultarIngredientePorNombre([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest(new { Message = "El nombre del ingrediente es inválido." });
            }

            try
            {
                IngredienteDTODriver ingredienteExistente = await _ingredientePort.ConsultarIngredienteNombre(nombre);

                if (ingredienteExistente == null)
                {
                    return NotFound(new { Message = $"No se encontró un ingrediente con el nombre '{nombre}'." });
                }

                return Ok(ingredienteExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error interno al consultar el ingrediente.", Details = ex.Message });
            }
        }
    }
}