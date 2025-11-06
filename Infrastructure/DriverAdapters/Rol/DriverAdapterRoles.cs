using Application.DTOs.Roles;
using Application.Ports.DriverPorts.Rol;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DriverAdapters.Rol
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        // 💡 Inyección: Inyectamos el puerto de entrada (IDriverPerfilPort).
        private readonly IDriverRolPort _rolPort;
        private readonly PortDriverRolCrear _rolPortCrear;
        

        public RolController(IDriverRolPort IDriverRolPort, PortDriverRolCrear rolPortCrear)
        {
            _rolPort = IDriverRolPort;
            _rolPortCrear = rolPortCrear;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarRolesExistentes([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest(new { Message = "El identificador de Rol es inválido." });
            }

            try
            {
                // 💡 Llamada al Puerto de Entrada: El Driver Adapter llama al puerto.
                List<RolDTODriven> rolesExistente = await _rolPort.ConsultarIdentificadoresRol(nombre);

                // 💡 Código Limpio: Retornamos la respuesta HTTP 200 OK con el resultado.
                return Ok(rolesExistente);
            }
            catch (Exception ex)
            {
                // Manejo de errores: Logs (no mostrado) y respuesta 500 para la seguridad.
                return StatusCode(500, new { Message = "Ocurrió un error interno al consultar los negocios.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearRol([FromBody] RolDTODriver rolCreacionDTO)
        {
            if (rolCreacionDTO == null)
            {
                return BadRequest(new { Message = "El cuerpo de la solicitud no puede estar vacío." });
            }

            if (string.IsNullOrWhiteSpace(rolCreacionDTO.Tipo) || string.IsNullOrWhiteSpace(rolCreacionDTO.Nombre))
            {
                return BadRequest(new { Message = "El Tipo y el Nombre del Rol son campos obligatorios." });
            }

            try
            {
                RolDTODriver CrearRol= await _rolPortCrear.CrearNuevoRol(rolCreacionDTO);

                return Created($"/api/rolcreado/{CrearRol.Identificacion}", CrearRol);
            }
            catch (ArgumentException ex)
            {
                // Manejo de errores de negocio (Ej: Rol ya existe) - 400 Bad Request
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Manejo de errores genéricos (Ej: Base de datos no disponible) - 500 Internal Server Error
                // Es crucial loguear el error aquí.
                return StatusCode(500, new { Message = "Ocurrió un error interno al crear el Rol.", Details = ex.Message });
            }
        }
    }
}