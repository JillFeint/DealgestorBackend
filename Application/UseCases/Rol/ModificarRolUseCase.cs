using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Rol
{
    public class ModificarRolUseCase : PortDriverRolModificar
    {
        private readonly PortDrivenRolModificar _drivenRolModificar;

        public ModificarRolUseCase(PortDrivenRolModificar drivenRolModificar)
        {
            _drivenRolModificar = drivenRolModificar ?? throw new ArgumentNullException(nameof(drivenRolModificar));
        }

        public async Task<RolDTODriver> ModificarRol(RolModificarRequestDTO rolModificarDTO)
        {
            var rolAModificar = await _drivenRolModificar.ObtenerRolNombreTipo(rolModificarDTO.NombreActual, rolModificarDTO.TipoActual);

            if (rolAModificar == null)
            {
                throw new Exception("El rol no existe.");
            }

            rolAModificar.Nombre = !string.IsNullOrWhiteSpace(rolModificarDTO.NuevoNombre) ? rolModificarDTO.NuevoNombre : rolAModificar.Nombre;
            rolAModificar.Tipo = !string.IsNullOrWhiteSpace(rolModificarDTO.NuevoTipo) ? rolModificarDTO.NuevoTipo : rolAModificar.Tipo;

            var modificadoRol = await _drivenRolModificar.ModificarRol(rolAModificar);

            if (modificadoRol == null)
            {
                throw new Exception("Error al modificar el rol en el repositorio.");
            }

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
