using Application.DTOs.Roles;
using Application.Ports.DriverPorts.Rol;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DriverAdapters.Rol
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("default")]
    [Authorize(Roles = "Admin")] // Solo Admin puede gestionar roles
    public class DriverAdapterRoles : ControllerBase
    {
        private readonly PortDriverRolConsultar _rolPortConsultar;
        private readonly PortDriverRolCrear _rolPortCrear;
        private readonly PortDriverRolEliminar _rolPortEliminar;
        private readonly PortDriverRolModificar _rolPortModificar;
        private readonly ILogger<DriverAdapterRoles> _logger;

        public DriverAdapterRoles(
            PortDriverRolConsultar rolPortConsultar, 
            PortDriverRolCrear rolPortCrear, 
            PortDriverRolEliminar rolPortEliminar, 
            PortDriverRolModificar rolPortModificar,
            ILogger<DriverAdapterRoles> logger)
        {
            _rolPortConsultar = rolPortConsultar ?? throw new ArgumentNullException(nameof(rolPortConsultar));
            _rolPortCrear = rolPortCrear ?? throw new ArgumentNullException(nameof(rolPortCrear));
            _rolPortEliminar = rolPortEliminar ?? throw new ArgumentNullException(nameof(rolPortEliminar));
            _rolPortModificar = rolPortModificar ?? throw new ArgumentNullException(nameof(rolPortModificar));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Consulta un rol existente por su nombre
        /// </summary>
        /// <param name="nombre">Nombre del rol a consultar</param>
        /// <returns>El rol consultado</returns>
        [HttpGet("consultar")]
        [ProducesResponseType(typeof(RolDTODriver), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarRolesExistentes([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                _logger.LogWarning("Intento de consultar rol con nombre vacío");
                return BadRequest(new { Message = "El identificador de Rol es inválido." });
            }

            try
            {
                RolDTODriver rolesExistente = await _rolPortConsultar.ConsultarIdentificadoresRol(nombre);

                if (rolesExistente == null)
                {
                    _logger.LogWarning("Rol no encontrado: {Nombre}", nombre);
                    return NotFound(new { Message = $"No se encontró un rol con el nombre '{nombre}'." });
                }

                return Ok(rolesExistente);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al consultar rol: {Nombre}", nombre);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar rol: {Nombre}", nombre);
                return StatusCode(500, new { Message = "Ocurrió un error interno al consultar los roles.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo rol en el sistema
        /// </summary>
        /// <param name="rolCreacionDTO">Datos del rol a crear</param>
        /// <returns>El rol creado</returns>
        [HttpPost("crear")] 
        [ProducesResponseType(typeof(RolDTODriver), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearRol([FromBody] RolDTODriver rolCreacionDTO)
        {
            // Validación automática usando Data Annotations
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación de modelo fallida al crear rol. Errores: {@Errores}", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new 
                { 
                    Message = "Los datos proporcionados no son válidos.",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                _logger.LogInformation("Creando rol: {@Rol}", new { rolCreacionDTO.Tipe, rolCreacionDTO.Name });
                
                RolDTODriver CrearRol = await _rolPortCrear.CrearNuevoRol(rolCreacionDTO);
                
                _logger.LogInformation("Rol creado exitosamente: {@RolCreado}", new { CrearRol.Identidad, CrearRol.Name });

                return Created($"/api/roles/{CrearRol.Identidad}", CrearRol);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al crear rol: {Nombre}", rolCreacionDTO.Name);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("ya existe"))
                {
                    _logger.LogWarning("Intento de crear rol duplicado: {Nombre}", rolCreacionDTO.Name);
                    return Conflict(new { Message = ex.Message });
                }
                
                _logger.LogWarning(ex, "Error de validación al crear rol: {Nombre}", rolCreacionDTO.Name);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al crear rol: {@Rol}", new { rolCreacionDTO.Tipe, rolCreacionDTO.Name });
                return StatusCode(500, new { Message = "Ocurrió un error interno al crear el Rol.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un rol existente por su nombre
        /// </summary>
        /// <param name="nombre">Nombre del rol a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("eliminar/{nombre}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarRol([FromRoute] string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                _logger.LogWarning("Intento de eliminar rol con nombre vacío");
                return BadRequest(new { Message = "El Identificador del Rol es un campo obligatorio." });
            }
            try
            {
                _logger.LogInformation("Eliminando rol: {Nombre}", nombre);
                
                bool rolEliminado = await _rolPortEliminar.EliminarRol(nombre);

                if (rolEliminado)
                {
                    DateTime fechaEliminacion = DateTime.UtcNow;
                    _logger.LogInformation("Rol eliminado exitosamente: {Nombre}", nombre);
                    return Ok(new { Message = "El rol ha sido eliminado exitosamente.", FechaEliminacion = fechaEliminacion });
                }
                else
                {
                    _logger.LogWarning("Intento de eliminar rol inexistente: {Nombre}", nombre);
                    return NotFound(new { Message = $"No se encontró un rol con el nombre '{nombre}'." });
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No se puede eliminar rol en uso: {Nombre}", nombre);
                return Conflict(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al eliminar rol: {Nombre}", nombre);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al eliminar rol: {Nombre}", nombre);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al eliminar rol: {Nombre}", nombre);
                return StatusCode(500, new { Message = "Ocurrió un error interno al eliminar el rol.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Modifica un rol existente
        /// </summary>
        /// <param name="rolModificarRequest">Datos del rol a modificar</param>
        /// <returns>El rol modificado</returns>
        [HttpPut("modificar")]
        [ProducesResponseType(typeof(RolDTODriver), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarRol([FromBody] RolModificarRequestDTO rolModificarRequest)
        {
            // Validación automática usando Data Annotations
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación de modelo fallida al modificar rol. Errores: {@Errores}", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new 
                { 
                    Message = "Los datos proporcionados no son válidos.",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                _logger.LogInformation("Modificando rol: {@RolActual}", new { rolModificarRequest.NombreActual, rolModificarRequest.TipoActual });
                
                RolDTODriver rolModificado = await _rolPortModificar.ModificarRol(rolModificarRequest);
                
                _logger.LogInformation("Rol modificado exitosamente: {@RolModificado}", new { rolModificado.Name, rolModificado.Tipe });

                return Ok(rolModificado);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al modificar rol: {Nombre}", rolModificarRequest.NombreActual);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("ya existe"))
                {
                    _logger.LogWarning("Intento de modificar rol a nombre duplicado: {Nombre}", rolModificarRequest.NombreActual);
                    return Conflict(new { Message = ex.Message });
                }
                
                _logger.LogWarning(ex, "Error de validación al modificar rol: {Nombre}", rolModificarRequest.NombreActual);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("El rol no existe"))
                {
                    _logger.LogWarning("Intento de modificar rol inexistente: {Nombre}", rolModificarRequest.NombreActual);
                    return NotFound(new { Message = ex.Message });
                }

                _logger.LogError(ex, "Error crítico al modificar rol: {Nombre}", rolModificarRequest.NombreActual);
                return StatusCode(500, new { Message = "Ocurrió un error interno al modificar el Rol.", Details = ex.Message });
            }
        }
    }
}