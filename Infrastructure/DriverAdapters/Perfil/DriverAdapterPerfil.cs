using Application.DTOs.Perfiles;
using Application.Ports.DriverPorts.Perfil;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DriverAdapters.Perfil
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverAdapterPerfil : ControllerBase
    {
        private readonly PortDriverPerfilCrear _crearPerfilUseCase;
        private readonly PortDriverPerfilConsultar _consultarPerfilUseCase;
        private readonly PortDriverPerfilEliminar _eliminarPerfilUseCase;
        private readonly PortDriverPerfilModificar _modificarPerfilUseCase;
        private readonly ILogger<DriverAdapterPerfil> _logger;

        public DriverAdapterPerfil(
            PortDriverPerfilCrear crearPerfilUseCase, 
            PortDriverPerfilConsultar consultarPerfilUseCase, 
            PortDriverPerfilEliminar eliminarPerfilUseCase,
            PortDriverPerfilModificar modificarPerfilUseCase,
            ILogger<DriverAdapterPerfil> logger)
        {
            _crearPerfilUseCase = crearPerfilUseCase ?? throw new ArgumentNullException(nameof(crearPerfilUseCase));
            _consultarPerfilUseCase = consultarPerfilUseCase ?? throw new ArgumentNullException(nameof(consultarPerfilUseCase));
            _eliminarPerfilUseCase = eliminarPerfilUseCase ?? throw new ArgumentNullException(nameof(eliminarPerfilUseCase));
            _modificarPerfilUseCase = modificarPerfilUseCase ?? throw new ArgumentNullException(nameof(modificarPerfilUseCase));
            _logger = logger;
        }

        /// <summary>
        /// Consulta un perfil de usuario por su email
        /// </summary>
        /// <param name="email">Email del perfil a consultar</param>
        /// <param name="codeEspecial">Código de acceso especial</param>
        /// <returns>El perfil consultado</returns>
        [HttpGet("consultarperfil")]
        [ProducesResponseType(typeof(PerfilRespuestaDTODriver), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarPerfil([FromQuery] string email, string codeEspecial)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Intento de consultar perfil con email vacío");
                return BadRequest(new { Message = "El email es requerido." });
            }

            try
            {
                PerfilRespuestaDTODriver perfil = await _consultarPerfilUseCase.ConsultarPerfilPorEmail(email, codeEspecial);

                if (perfil == null)
                {
                    _logger.LogWarning("Perfil no encontrado: {Email}", email);
                    return NotFound(new { Message = $"No se encontró un perfil con el email '{email}'." });
                }

                return Ok(perfil);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al consultar perfil: {Email}", email);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al consultar perfil: {Email}", email);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar perfil: {Email}", email);
                return StatusCode(500, new { Message = "Error interno del servidor al consultar el perfil.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Registra un nuevo perfil de usuario en el sistema
        /// </summary>
        /// <param name="nuevoPerfilDTO">Datos del perfil a registrar</param>
        /// <returns>El perfil creado</returns>
        [HttpPost("registrarperfil")]
        [ProducesResponseType(typeof(PerfilRespuestaDTODriver), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarPerfil([FromBody] PerfilIngresoDTODriver nuevoPerfilDTO)
        {
            // Validación automática usando Data Annotations
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación de modelo fallida al registrar perfil. Errores: {@Errores}", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new 
                { 
                    Message = "Los datos proporcionados no son válidos.",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                _logger.LogInformation("Registrando perfil: {@Perfil}", new { nuevoPerfilDTO.Email });
                
                PerfilRespuestaDTODriver perfilCreado = await _crearPerfilUseCase.CrearNuevoPerfil(nuevoPerfilDTO);
                
                _logger.LogInformation("Perfil registrado exitosamente: {@PerfilCreado}", new { perfilCreado.Email, perfilCreado.FechaCreacion });

                return Created($"/api/perfil/{perfilCreado.Email}", perfilCreado);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al registrar perfil: {Email}", nuevoPerfilDTO.Email);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("ya existe"))
                {
                    _logger.LogWarning("Intento de registrar perfil duplicado: {Email}", nuevoPerfilDTO.Email);
                    return Conflict(new { Message = ex.Message });
                }
                
                _logger.LogWarning(ex, "Error de validación al registrar perfil: {Email}", nuevoPerfilDTO.Email);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al registrar perfil: {Email}", nuevoPerfilDTO.Email);
                return StatusCode(500, new { Message = "Error interno del servidor al crear el perfil.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un perfil de usuario por su email
        /// </summary>
        /// <param name="email">Email del perfil a eliminar</param>
        /// <param name="codeEspecial">Código de acceso especial para autorizar la eliminación</param>
        /// <returns>Email del perfil eliminado y fecha de eliminación</returns>
        [HttpDelete("eliminarperfil/{email}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarPerfil([FromRoute] string email, [FromQuery] string codeEspecial)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Intento de eliminar perfil con email vacío");
                return BadRequest(new { Message = "El email es requerido." });
            }

            try
            {
                _logger.LogInformation("Eliminando perfil: {Email}", email);
                
                bool eliminado = await _eliminarPerfilUseCase.EliminarPerfil(email, codeEspecial);
                
                if (eliminado)
                {
                    DateTime fechaEliminacion = DateTime.UtcNow;
                    _logger.LogInformation("Perfil eliminado exitosamente: {Email}", email);
                    return Ok(new { Email = email, FechaEliminacion = fechaEliminacion });
                }
                
                _logger.LogWarning("Intento de eliminar perfil inexistente: {Email}", email);
                return NotFound(new { Message = $"No se encontró un perfil con el email '{email}'." });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al eliminar perfil: {Email}", email);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al eliminar perfil: {Email}", email);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al eliminar perfil: {Email}", email);
                return StatusCode(500, new { Message = "Error interno del servidor al eliminar el perfil.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Modifica un perfil de usuario existente
        /// </summary>
        /// <param name="perfilModificarRequest">Datos del perfil a modificar</param>
        /// <returns>El perfil modificado</returns>
        [HttpPut("modificarperfil")]
        [ProducesResponseType(typeof(PerfilRespuestaDTODriver), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarPerfil([FromBody] PerfilModificarRequestDTO perfilModificarRequest)
        {
            // Validación automática usando Data Annotations
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación de modelo fallida al modificar perfil. Errores: {@Errores}", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new 
                { 
                    Message = "Los datos proporcionados no son válidos.",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                _logger.LogInformation("Modificando perfil: {@PerfilActual}", new { perfilModificarRequest.EmailActual });
                
                PerfilRespuestaDTODriver perfilModificado = await _modificarPerfilUseCase.ModificarPerfil(perfilModificarRequest);
                
                _logger.LogInformation("Perfil modificado exitosamente: {@PerfilModificado}", new { perfilModificado.Email });

                return Ok(perfilModificado);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al modificar perfil: {Email}", perfilModificarRequest.EmailActual);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("ya existe"))
                {
                    _logger.LogWarning("Intento de modificar perfil a email duplicado: {Email}", perfilModificarRequest.EmailActual);
                    return Conflict(new { Message = ex.Message });
                }
                
                _logger.LogWarning(ex, "Error de validación al modificar perfil: {Email}", perfilModificarRequest.EmailActual);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no existe"))
                {
                    _logger.LogWarning("Intento de modificar perfil inexistente: {Email}", perfilModificarRequest.EmailActual);
                    return NotFound(new { Message = ex.Message });
                }

                _logger.LogError(ex, "Error crítico al modificar perfil: {Email}", perfilModificarRequest.EmailActual);
                return StatusCode(500, new { Message = "Error interno del servidor al modificar el perfil.", Details = ex.Message });
            }
        }
    }
}

