using Application.DTOs.Ingredientes;
using Application.Ports.DriverPorts.Ingrediente;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
        private readonly ILogger<DriverAdapterIngredientes> _logger;

        public DriverAdapterIngredientes(
            PortDriverIngredienteConsultar ingredientePortConsultar, 
            PortDriverIngredienteCrear ingredientePortCrear, 
            PortDriverIngredienteEliminar ingredientePortEliminar, 
            PortDriverIngredienteModificar ingredientePortModificar,
            ILogger<DriverAdapterIngredientes> logger)
        {
            _ingredientePortConsultar = ingredientePortConsultar ?? throw new ArgumentNullException(nameof(ingredientePortConsultar));
            _ingredientePortCrear = ingredientePortCrear ?? throw new ArgumentNullException(nameof(ingredientePortCrear));
            _ingredientePortEliminar = ingredientePortEliminar ?? throw new ArgumentNullException(nameof(ingredientePortEliminar));
            _ingredientePortModificar = ingredientePortModificar ?? throw new ArgumentNullException(nameof(ingredientePortModificar));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Consulta un ingrediente por su nombre
        /// </summary>
        /// <param name="nombre">Nombre del ingrediente a consultar</param>
        /// <returns>El ingrediente consultado</returns>
        [HttpGet("consultar")]
        [ProducesResponseType(typeof(IngredienteDTODriver), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarIngredientePorNombre([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                _logger.LogWarning("Intento de consultar ingrediente con nombre vacío");
                return BadRequest(new { Message = "El nombre del ingrediente es inválido." });
            }

            try
            {
                IngredienteDTODriver ingredienteExistente = await _ingredientePortConsultar.ConsultarIngredienteNombre(nombre);

                if (ingredienteExistente == null)
                {
                    _logger.LogWarning("Ingrediente no encontrado: {Nombre}", nombre);
                    return NotFound(new { Message = $"No se encontró un ingrediente con el nombre '{nombre}'." });
                }

                return Ok(ingredienteExistente);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al consultar ingrediente: {Nombre}", nombre);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar ingrediente: {Nombre}", nombre);
                return StatusCode(500, new { Message = "Ocurrió un error interno al consultar el ingrediente.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo ingrediente en el sistema
        /// </summary>
        /// <param name="ingrediente">Datos del ingrediente a crear</param>
        /// <returns>El ingrediente creado</returns>
        [HttpPost("crear")]
        [ProducesResponseType(typeof(IngredienteDTODriver), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearIngrediente([FromBody] IngredienteDTODriver ingrediente)
        { 
            // Validación automática usando Data Annotations
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación de modelo fallida al crear ingrediente. Errores: {@Errores}", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new 
                { 
                    Message = "Los datos proporcionados no son válidos.",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            
            try
            {
                _logger.LogInformation("Creando ingrediente: {@Ingrediente}", 
                    new { ingrediente.NameIngredient, ingrediente.Ref });
                
                IngredienteDTODriver ingredienteCreado = await _ingredientePortCrear.CrearNuevoIngrediente(ingrediente);
                
                _logger.LogInformation("Ingrediente creado exitosamente: {@IngredienteCreado}", 
                    new { ingredienteCreado.Ref, ingredienteCreado.NameIngredient });
                
                return Created($"/api/ingredientes/{ingredienteCreado.Ref}", ingredienteCreado);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al crear ingrediente: {Nombre}", ingrediente.NameIngredient);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("ya existe"))
                {
                    _logger.LogWarning("Intento de crear ingrediente duplicado: {Nombre}", ingrediente.NameIngredient);
                    return Conflict(new { Message = ex.Message });
                }
                
                _logger.LogWarning(ex, "Error de validación al crear ingrediente: {Nombre}", ingrediente.NameIngredient);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al crear ingrediente: {@Ingrediente}", 
                    new { ingrediente.NameIngredient, ingrediente.Ref });
                return StatusCode(500, new { Message = "Ocurrió un error interno al crear el ingrediente.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un ingrediente por su referencia
        /// </summary>
        /// <param name="referencia">Referencia del ingrediente a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("{referencia}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarIngrediente([FromRoute] int referencia)
        {
            if (referencia <= 0)
            {
                _logger.LogWarning("Intento de eliminar ingrediente con referencia inválida: {Ref}", referencia);
                return BadRequest(new { Message = "La referencia del ingrediente es inválida." });
            }

            try
            {
                _logger.LogInformation("Eliminando ingrediente con Ref: {Ref}", referencia);
                
                bool ingredienteEliminado = await _ingredientePortEliminar.EliminarIngrediente(referencia);

                if (ingredienteEliminado)
                {
                    DateTime fechaEliminacion = DateTime.UtcNow;
                    _logger.LogInformation("Ingrediente eliminado exitosamente: Ref {Ref}", referencia);
                    return Ok(new { Message = "El ingrediente ha sido eliminado exitosamente.", FechaEliminacion = fechaEliminacion });
                }
                else
                {
                    _logger.LogWarning("Intento de eliminar ingrediente inexistente: Ref {Ref}", referencia);
                    return NotFound(new { Message = $"No se encontró un ingrediente con la referencia '{referencia}'." });
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al eliminar ingrediente: Ref {Ref}", referencia);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al eliminar ingrediente: Ref {Ref}", referencia);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al eliminar ingrediente: Ref {Ref}", referencia);
                return StatusCode(500, new { Message = "Ocurrió un error interno al eliminar el ingrediente.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Modifica un ingrediente existente
        /// </summary>
        /// <param name="ingredienteXModificar">Datos del ingrediente a modificar</param>
        /// <returns>El ingrediente modificado</returns>
        [HttpPut("modificar/{referencia}")]
        [ProducesResponseType(typeof(IngredienteDTODriver), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarIngrediente([FromBody] IngredienteDTODriver ingredienteXModificar)
        {
            // Validación automática usando Data Annotations
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación de modelo fallida al modificar ingrediente. Errores: {@Errores}", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new 
                { 
                    Message = "Los datos proporcionados no son válidos.",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                _logger.LogInformation("Modificando ingrediente: {@IngredienteActual}", 
                    new { ingredienteXModificar.Ref, ingredienteXModificar.NameIngredient });
                
                IngredienteDTODriver ingredienteModificado = await _ingredientePortModificar.ModificarIngrediente(ingredienteXModificar);
                
                _logger.LogInformation("Ingrediente modificado exitosamente: {@IngredienteModificado}", 
                    new { ingredienteModificado.Ref, ingredienteModificado.NameIngredient });

                return Ok(ingredienteModificado);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al modificar ingrediente: Ref {Ref}", ingredienteXModificar.Ref);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("ya existe"))
                {
                    _logger.LogWarning("Intento de modificar ingrediente a nombre duplicado: Ref {Ref}", ingredienteXModificar.Ref);
                    return Conflict(new { Message = ex.Message });
                }
                
                _logger.LogWarning(ex, "Error de validación al modificar ingrediente: Ref {Ref}", ingredienteXModificar.Ref);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {       
                if (ex.Message.Contains("no existe"))
                {
                    _logger.LogWarning("Intento de modificar ingrediente inexistente: Ref {Ref}", ingredienteXModificar.Ref);
                    return NotFound(new { Message = ex.Message });
                }

                _logger.LogError(ex, "Error crítico al modificar ingrediente: Ref {Ref}", ingredienteXModificar.Ref);
                return StatusCode(500, new { Message = "Ocurrió un error interno al modificar el ingrediente.", Details = ex.Message });
            }
        }
    }
}