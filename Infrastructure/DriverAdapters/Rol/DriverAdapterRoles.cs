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
    public class DriverAdapterRoles : ControllerBase
    {
        // 💡 Inyección: Inyectamos el puerto de entrada (IDriverPerfilPort).
        private readonly PortDriverRolConsultar _rolPort;
        private readonly PortDriverRolCrear _rolPortCrear;
        private readonly PortDriverRolEliminar _rolPortEliminar;
        private readonly PortDriverRolModificar _rolPortModificar;

        public DriverAdapterRoles(PortDriverRolConsultar DriverRolPort, PortDriverRolCrear rolPortCrear, PortDriverRolEliminar rolPortEliminar, PortDriverRolModificar rolPortModificar)
        {
            _rolPort = DriverRolPort;
            _rolPortCrear = rolPortCrear;
            _rolPortEliminar = rolPortEliminar;
            _rolPortModificar = rolPortModificar;
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> ConsultarRolesExistentes([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest(new { Message = "El identificador de Rol es inválido." });
            }

            try
            {
                RolDTODriver rolesExistente = await _rolPort.ConsultarIdentificadoresRol(nombre);

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
                RolDTODriver CrearRol = await _rolPortCrear.CrearNuevoRol(rolCreacionDTO);

                return Created($"/api/rolcreado/{CrearRol.Identificacion}", CrearRol);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error interno al crear el Rol.", Details = ex.Message });
            }
        }

        [HttpDelete("{nombre}")]
        public async Task<IActionResult> EliminarRol([FromRoute] string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest(new { Message = "El Identificador del Rol es un campo obligatorio." });
            }
            try
            {
                bool rolEliminado = await _rolPortEliminar.EliminarRol(nombre);

                if (rolEliminado)
                {

                    return Ok(new { Message = "Ha sido eliminado exitosamente." });
                }
                else
                {
                    return NotFound(new { Message = "Sin proceso" });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error interno al eliminar.", Details = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> ModificarRol([FromBody] RolDTODriver rolModificarDTO)
        {
            if (rolModificarDTO == null)
            {
                return BadRequest(new { Message = "El cuerpo de la solicitud no puede estar vacío." });
            }

            if (string.IsNullOrWhiteSpace(rolModificarDTO.Tipo) || string.IsNullOrWhiteSpace(rolModificarDTO.Nombre))
            {
                return BadRequest(new { Message = "El Tipo y el Nombre del Rol son campos obligatorios para la modificación." });
            }

            try
            {
                RolDTODriver rolModificado = await _rolPortModificar.ModificarRol(rolModificarDTO);
                return Ok(rolModificado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error interno al modificar el Rol.", Details = ex.Message });
            }
        }
    }
}