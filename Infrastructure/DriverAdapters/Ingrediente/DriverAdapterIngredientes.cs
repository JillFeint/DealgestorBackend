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
        private readonly PortDriverIngredienteConsultar _ingredientePortConsultar;
        private readonly PortDriverIngredienteCrear _ingredientePortCrear;
        private readonly PortDriverIngredienteEliminar _ingredientePortEliminar;
        private readonly PortDriverIngredienteModificar _ingredientePortModificar;

        public DriverAdapterIngredientes(PortDriverIngredienteConsultar ingredientePortConsultar, PortDriverIngredienteCrear ingredientePortCrear, PortDriverIngredienteEliminar ingredientePortEliminar, PortDriverIngredienteModificar ingredientePortModificar)
        {
            _ingredientePortConsultar = ingredientePortConsultar ?? throw new ArgumentNullException(nameof(ingredientePortConsultar));
            _ingredientePortCrear = ingredientePortCrear ?? throw new ArgumentNullException(nameof(ingredientePortCrear));
            _ingredientePortEliminar = ingredientePortEliminar ?? throw new ArgumentNullException(nameof(ingredientePortEliminar));
            _ingredientePortModificar = ingredientePortModificar ?? throw new ArgumentNullException(nameof(ingredientePortModificar));
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
        public async Task<IActionResult> CrearIngrediente([FromBody] IngredienteDTODriver ingrediente)
        { 
         if (ingrediente == null)
            {
                return BadRequest(new { Message = "Los datos del ingrediente son inválidos." });
            }
            try
            {
                IngredienteDTODriver ingredienteCreado = await _ingredientePortCrear.CrearNuevoIngrediente(ingrediente);
                return Ok(ingredienteCreado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error interno al crear el ingrediente.", Details = ex.Message });
            }
        }

        [HttpDelete("{referencia}")]
        public async Task<IActionResult> EliminarIngrediente([FromRoute] int referencia)
        {
            if (referencia <= 0)
            {
                return BadRequest(new { Message = "La referencia del ingrediente es inválida." });
            }

            try
            {
                bool ingredienteEliminado = await _ingredientePortEliminar.EliminarIngrediente(referencia);

                if (ingredienteEliminado)
                {

                    return Ok(new { Message = "Ha sido eliminado exitosamente." });
                }
                else
                {
                    return NotFound(new { Message = "Sin proceso" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error interno al eliminar el ingrediente.", Details = ex.Message });
            }
        }

        [HttpPut("{referencia}")]
        public async Task<IActionResult> ModificarIngrediente([FromBody] IngredienteDTODriver ingredienteXModificar)
        {
            if (ingredienteXModificar == null)
            {
                return BadRequest(new { Message = "El cuerpo de la solicitud no puede ser null." });
            }

            try
            {
                IngredienteDTODriver ingredienteModificado = await _ingredientePortModificar.ModificarIngrediente(ingredienteXModificar);

                return Ok(ingredienteModificado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {       
                if (ex.Message.Contains("El rol no existe"))
                    return NotFound(new { Message = ex.Message });

                return StatusCode(500, new { Message = "Ocurrió un error interno al modificar el Rol.", Details = ex.Message });
            }
        }
    }
}