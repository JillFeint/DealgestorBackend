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
            if (rolModificarDTO == null) throw new ArgumentNullException(nameof(rolModificarDTO));

            string nombreActualCmp = Normalizar(rolModificarDTO.NombreActual);
            string tipoActualCmp = Normalizar(rolModificarDTO.TipoActual);

            var rolAModificar = await _drivenRolModificar.ObtenerRolNombreTipo(nombreActualCmp, tipoActualCmp);

            if (rolAModificar == null)
            {
                throw new ArgumentException("El rol no existe.");
            }

            string nuevoNombre = string.IsNullOrWhiteSpace(rolModificarDTO.NuevoNombre)
                ? rolAModificar.Nombre
                : rolModificarDTO.NuevoNombre.Trim();

            string nuevoTipo = string.IsNullOrWhiteSpace(rolModificarDTO.NuevoTipo)
                ? rolAModificar.Tipo
                : rolModificarDTO.NuevoTipo.Trim();

            string nuevoNombreCmp = Normalizar(nuevoNombre);
            string nuevoTipoCmp = Normalizar(nuevoTipo);

            var rolDuplicado = await _drivenRolModificar.ObtenerRolNombreTipo(nuevoNombreCmp, nuevoTipoCmp);
            if (rolDuplicado != null && rolDuplicado.Identificacion != rolAModificar.Identificacion)
            {
                throw new ArgumentException($"El rol '{nuevoNombre}' ya existe.");
            }

            rolAModificar.Nombre = nuevoNombre;
            rolAModificar.Tipo = nuevoTipo;

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

        private string Normalizar(string value) => value.Trim().ToUpperInvariant();
    }
}
