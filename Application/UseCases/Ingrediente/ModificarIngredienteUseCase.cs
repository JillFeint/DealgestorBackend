using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Rol
{
    public class ModificarIngredienteUseCase : PortDriverIngredienteModificar
    {
        private readonly PortDrivenIngredienteModificar _drivenRolModificar;

        public ModificarIngredienteUseCase(PortDrivenIngredienteModificar drivenRolModificar)
        {
            _drivenRolModificar = drivenRolModificar ?? throw new ArgumentNullException(nameof(drivenRolModificar));
        }

        public async Task<RolDTODriver> ModificarRol(RolModificarRequestDTO rolModificarDTO)
        {
            var rolAModificar = await _drivenRolModificar.ObtenerRolPorNombreTipo(rolModificarDTO.NombreActual, rolModificarDTO.TipoActual);

            if (rolAModificar == null)
            {
                throw new Exception("El rol no existe.");
            }

            // 2. Aplicar los cambios a la entidad de dominio.
            // Usamos ?? para mantener el valor original si el nuevo valor es nulo o vacío.
            rolAModificar.Nombre = !string.IsNullOrWhiteSpace(rolModificarDTO.NuevoNombre) ? rolModificarDTO.NuevoNombre : rolAModificar.Nombre;
            rolAModificar.Tipo = !string.IsNullOrWhiteSpace(rolModificarDTO.NuevoTipo) ? rolModificarDTO.NuevoTipo : rolAModificar.Tipo;

            // 3. Llamar al puerto driven para persistir la entidad modificada.
            var modificadoRol = await _drivenRolModificar.ModificarRol(rolAModificar);

            if (modificadoRol == null)
            {
                throw new Exception("Error al modificar el rol en el repositorio.");
            }

            // 4. Mapear la entidad de dominio actualizada de nuevo a un DTO para la respuesta.
            var resultaDTO = new RolDTODriver
            {
                Identidad = modificadoRol.Identificacion,
                Tipe = modificadoRol.Tipo,
                Name = modificadoRol.Nombre
            };

            return resultaDTO;
        }
    }
}
