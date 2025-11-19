using Application.DTOs.Ingredientes;
using Application.Ports.DriverPorts.Ingrediente;
using Application.Ports.DriverPorts.Rol;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DriverAdapters.Ingrediente
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverAdapterIngredientes : ControllerBase
    {
        private readonly PortDriverIngredienteConsultar _ingredientePortConsultar;
        private readonly PortDriverIngredienteCrear _ingredientePortCrear;

        public DriverAdapterIngredientes(PortDriverIngredienteConsultar ingredientePortConsultar, PortDriverIngredienteCrear ingredientePortCrear)
        {
            _ingredientePortConsultar = ingredientePortConsultar ?? throw new ArgumentNullException(nameof(ingredientePortConsultar));
            _ingredientePortCrear = ingredientePortCrear ?? throw new ArgumentNullException(nameof(ingredientePortCrear)); 
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
                IngredienteDTODriver ingredienteExistente = await _ingredientePortConsultar.ConsultarIngredienteNombre(nombre);

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

        [HttpPost("crear")]
        public async Task<IActionResult> CrearIngrediente([FromQuery] IngredienteDTODriver ingrediente)
        { 
         if (ingrediente == null)
            {
                return BadRequest(new { Message = "Los datos del ingrediente son inválidos." });
            }
            try
            {
                IngredienteDTODriver ingredienteCreado = await _ingredientePortCrear.CrearIngrediente(ingrediente);
                return Ok(ingredienteCreado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error interno al crear el ingrediente.", Details = ex.Message });
            }
        }
    }
}