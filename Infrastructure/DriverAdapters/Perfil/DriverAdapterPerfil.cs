using Application.DTOs.Perfiles;
using Application.Ports.DriverPorts.Perfil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DriverAdapters.Perfil
{
    [ApiController]
    [Route("api/[controller]")]         
    [EnableRateLimiting("default")]
    public class DriverAdapterPerfil : ControllerBase
    {
        private readonly PortDriverPerfilCrear _perfilPortCrear;
        private readonly PortDriverPerfilConsultar _perfilPortConsultar;
        private readonly PortDriverPerfilEliminar _perfilPortEliminar;
        private readonly PortDriverPerfilModificar _perfilPortModificar;
        private readonly PortDriverPerfilAutenticar _perfilPortAutenticar;
        private readonly ILogger<DriverAdapterPerfil> _logger;

        public DriverAdapterPerfil(
            PortDriverPerfilCrear perfilPortCrear, 
            PortDriverPerfilConsultar perfilPortConsultar, 
            PortDriverPerfilEliminar perfilPortEliminar,
            PortDriverPerfilModificar perfilPortModificar,
            PortDriverPerfilAutenticar perfilPortAutenticar,
            ILogger<DriverAdapterPerfil> logger)
        {
            _perfilPortCrear = perfilPortCrear ?? throw new ArgumentNullException(nameof(perfilPortCrear));
            _perfilPortConsultar = perfilPortConsultar ?? throw new ArgumentNullException(nameof(perfilPortConsultar));
            _perfilPortEliminar = perfilPortEliminar ?? throw new ArgumentNullException(nameof(perfilPortEliminar));
            _perfilPortModificar = perfilPortModificar ?? throw new ArgumentNullException(nameof(perfilPortModificar));
            _perfilPortAutenticar = perfilPortAutenticar ?? throw new ArgumentNullException(nameof(perfilPortAutenticar));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Autentica un perfil de usuario y genera un token JWT.
        /// Este endpoint permite a los usuarios iniciar sesión con su email y contraseña.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous] 
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación de modelo fallida en login. Errores: {@Errores}", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new 
                { 
                    Message = "Los datos proporcionados no son válidos.",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                _logger.LogInformation("Intento de autenticación para email: {Email}", loginRequest.Email);
                
                LoginResponseDTO resultado = await _perfilPortAutenticar.AutenticarPerfil(loginRequest);
                
                _logger.LogInformation("Autenticación exitosa para email: {Email}. Roles: {@Roles}", 
                    resultado.Email, resultado.Roles);

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación en login para email: {Email}", loginRequest.Email);
                return BadRequest(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Contraseña incorrecta para email: {Email}", loginRequest.Email);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico durante autenticación para email: {Email}", loginRequest.Email);
                return StatusCode(500, new { Message = "Error interno del servidor durante la autenticación.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Consulta un perfil de usuario por su email.
        /// Requiere estar autenticado (cualquier rol).
        /// </summary>
        [HttpGet("consultarperfil")]
        [Authorize] 
        [ProducesResponseType(typeof(PerfilRespuestaDTODriver), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarPerfil([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Intento de consultar perfil con email vacío");
                return BadRequest(new { Message = "El email es requerido." });
            }

            try
            {
                PerfilRespuestaDTODriver perfil = await _perfilPortConsultar.ConsultarPerfilPorEmail(email);

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
        /// Registra un nuevo perfil de usuario en el sistema.
        /// No requiere autenticación previa (público para registro).
        /// </summary>
        [HttpPost("registrarperfil")]
        [AllowAnonymous] 
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
                
                PerfilRespuestaDTODriver perfilCreado = await _perfilPortCrear.CrearNuevoPerfil(nuevoPerfilDTO);
                
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
        /// Elimina un perfil de usuario por su email.
        /// Requiere rol Admin (acción crítica).
        /// </summary>
        [HttpDelete("eliminarperfil/{email}")]
        [Authorize(Roles = "Admin")] // Solo Admin puede eliminar perfiles
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarPerfil([FromRoute] string email, [FromHeader(Name = "X-Code-Especial")] string codeEspecial)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Intento de eliminar perfil con email vacío");
                return BadRequest(new { Message = "El email es requerido." });
            }

            try
            {
                _logger.LogInformation("Eliminando perfil: {Email}", email);
                
                bool eliminado = await _perfilPortEliminar.EliminarPerfil(email, codeEspecial);
                
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
        /// Modifica un perfil de usuario existente.
        /// Requiere estar autenticado (cualquier rol puede modificar su propio perfil).
        /// </summary>
        [HttpPut("modificarperfil")]
        [Authorize] // Requiere estar autenticado
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
                
                PerfilRespuestaDTODriver perfilModificado = await _perfilPortModificar.ModificarPerfil(perfilModificarRequest);
                
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




