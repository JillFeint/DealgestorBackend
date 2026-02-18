using Application.DTOs.Productos;
using Application.Ports.DriverPorts.Producto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DriverAdapters.Producto
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("default")]
    public class DriverAdapterProducto : ControllerBase
    {
        private readonly PortDriverProductoConsultar _productoPortConsultar;                    
        private readonly PortDriverProductoCrear _productoPortCrear;
        private readonly PortDriverProductoEliminar _productoPortEliminar;
        private readonly PortDriverProductoModificar _productoPortModificar;
        private readonly ILogger<DriverAdapterProducto> _logger;

        public DriverAdapterProducto(           
            PortDriverProductoConsultar productoPortConsultar,
            PortDriverProductoCrear productoPortCrear,
            PortDriverProductoEliminar productoPortEliminar,
            PortDriverProductoModificar productoPortModificar,
            ILogger<DriverAdapterProducto> logger)
        {
            _productoPortConsultar = productoPortConsultar ?? throw new ArgumentNullException(nameof(productoPortConsultar));
            _productoPortCrear = productoPortCrear ?? throw new ArgumentNullException(nameof(productoPortCrear));
            _productoPortEliminar = productoPortEliminar ?? throw new ArgumentNullException(nameof(productoPortEliminar));
            _productoPortModificar = productoPortModificar ?? throw new ArgumentNullException(nameof(productoPortModificar));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Consulta un producto existente por su nombre.
        /// Público - no requiere autenticación (permite ver catálogo).
        /// </summary>
        /// <param name="nombre">Nombre del producto a consultar</param>
        /// <returns>El producto consultado</returns>
        [HttpGet("consultar")]
        [AllowAnonymous] // Público - cualquiera puede ver productos
        [ProducesResponseType(typeof(ProductoDTODriver), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarProductoExistente([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                _logger.LogWarning("Intento de consultar producto con nombre vacío");
                return BadRequest(new { Message = "El nombre del producto es inválido." });
            }

            try
            {
                ProductoDTODriver productoExistente = await _productoPortConsultar.ConsultarProductoNombre(nombre);

                if (productoExistente == null)
                {
                    _logger.LogWarning("Producto no encontrado: {Nombre}", nombre);
                    return NotFound(new { Message = $"No se encontró un producto con el nombre '{nombre}'." });
                }

                return Ok(productoExistente);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al consultar producto: {Nombre}", nombre);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar producto: {Nombre}", nombre);
                return StatusCode(500, new { Message = "Ocurrió un error interno al consultar el producto.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo producto en el sistema.
        /// Requiere rol Admin o Vendedor.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Vendedor")]
        [ProducesResponseType(typeof(ProductoDTODriver), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoDTODriver nuevoProducto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación de modelo fallida al crear producto. Errores: {@Errores}",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new
                {
                    Message = "Los datos proporcionados no son válidos.",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                _logger.LogInformation("Creando producto: {@Producto}",
                    new { nuevoProducto.Nombre, nuevoProducto.Referencia });

                ProductoDTODriver productoCreado = await _productoPortCrear.CrearNuevoProducto(nuevoProducto);

                _logger.LogInformation("Producto creado exitosamente: {@ProductoCreado}",
                    new { productoCreado.Referencia, productoCreado.Nombre });

                return Created($"/api/producto/{productoCreado.Referencia}", productoCreado);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al crear producto: {Nombre}", nuevoProducto.Nombre);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("ya existe"))
                {
                    _logger.LogWarning("Intento de crear producto duplicado: Ref {Referencia}", nuevoProducto.Referencia);
                    return Conflict(new { Message = ex.Message });
                }

                _logger.LogWarning(ex, "Error de validación al crear producto: {Nombre}", nuevoProducto.Nombre);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al crear producto: {@Producto}",
                    new { nuevoProducto.Nombre, nuevoProducto.Referencia });
                return StatusCode(500, new { Message = "Error interno al crear el producto.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Modifica un producto existente en el sistema.
        /// Requiere rol Admin o Vendedor.
        /// </summary>
        [HttpPut("{referencia}")]
        [Authorize(Roles = "Admin,Vendedor")]
        [ProducesResponseType(typeof(ProductoDTODriver), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarProducto(int referencia, [FromBody] ProductoDTODriver productoModificado)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación de modelo fallida al modificar producto. Errores: {@Errores}",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new
                {
                    Message = "Los datos proporcionados no son válidos.",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                _logger.LogInformation("Modificando producto: {@ProductoActual}",
                    new { productoModificado.Referencia, productoModificado.Nombre });

                ProductoDTODriver productoModificadoRespuesta = await _productoPortModificar.ModificarProducto(productoModificado);

                _logger.LogInformation("Producto modificado exitosamente: {@ProductoModificado}",
                    new { productoModificadoRespuesta.Referencia, productoModificadoRespuesta.Nombre });

                return Ok(productoModificadoRespuesta);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al modificar producto: Ref {Ref}", productoModificado.Referencia);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("ya existe"))
                {
                    _logger.LogWarning("Intento de modificar producto a nombre duplicado: Ref {Ref}", productoModificado.Referencia);
                    return Conflict(new { Message = ex.Message });
                }

                _logger.LogWarning(ex, "Error de validación al modificar producto: Ref {Ref}", productoModificado.Referencia);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no existe"))
                {
                    _logger.LogWarning("Intento de modificar producto inexistente: Ref {Ref}", productoModificado.Referencia);
                    return NotFound(new { Message = ex.Message });
                }

                _logger.LogError(ex, "Error crítico al modificar producto: Ref {Ref}", productoModificado.Referencia);
                return StatusCode(500, new { Message = "Ocurrió un error interno al modificar el producto.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un producto existente del sistema.
        /// Requiere rol Admin o Vendedor.
        /// </summary>
        /// <param name="referencia">Referencia del producto a eliminar</param>
        /// <returns>Mensaje de éxito o error</returns>
        [HttpDelete("{referencia}")]
        [Authorize(Roles = "Admin,Vendedor")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarProducto(int referencia)
        {
            if (referencia <= 0)
            {
                _logger.LogWarning("Intento de eliminar producto con referencia inválida");
                return BadRequest(new { Message = "La referencia del producto es inválida." });
            }

            try
            {
                _logger.LogInformation("Eliminando producto con referencia: {Ref}", referencia);

                bool productoEliminado = await _productoPortEliminar.EliminarProducto(referencia);

                if (productoEliminado)
                {
                    DateTime fechaEliminacion = DateTime.UtcNow;
                    _logger.LogInformation("Producto eliminado exitosamente: Ref {Ref}", referencia);
                    return Ok(new { Message = "El producto ha sido eliminado exitosamente.", FechaEliminacion = fechaEliminacion });
                }

                _logger.LogWarning("Intento de eliminar producto inexistente: Ref {Ref}", referencia);
                return NotFound(new { Message = $"No se encontró un producto con la referencia '{referencia}'." });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado al eliminar producto: Ref {Ref}", referencia);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al eliminar producto: Ref {Ref}", referencia);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al eliminar producto: Ref {Ref}", referencia);
                return StatusCode(500, new { Message = "Ocurrió un error interno al eliminar el producto.", Details = ex.Message });
            }
        }
    }
}



