using Application.DTOs.Perfiles;
using Application.Ports.DrivenPorts.Perfil;
using Application.Ports.DriverPorts.Perfil;
using Application.Services;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Perfil
{
    public class ModificarPerfilUseCase : PortDriverPerfilModificar
    {
        private readonly PortDrivenPerfilModificar _drivenPerfilModificar;

        public ModificarPerfilUseCase(PortDrivenPerfilModificar drivenPerfilModificar)
        {
            _drivenPerfilModificar = drivenPerfilModificar ?? throw new ArgumentNullException(nameof(drivenPerfilModificar));
        }

        public async Task<PerfilRespuestaDTODriver> ModificarPerfil(PerfilModificarRequestDTO perfilModificarDTO)
        {
            var perfilAModificar = await _drivenPerfilModificar.ObtenerPerfilPorEmail(perfilModificarDTO.EmailActual);

            if (perfilAModificar == null)
            {
                throw new Exception("El perfil no existe.");
            }

            // Verificar código secreto actual
            if (!await PasswordHasher.VerifyPasswordAsync(perfilModificarDTO.CodigoSecretoActual, perfilAModificar.tblCodigoSecreto))
            {
                throw new UnauthorizedAccessException("El código secreto actual es incorrecto.");
            }

            // Actualizar email si se proporciona uno nuevo
            if (!string.IsNullOrWhiteSpace(perfilModificarDTO.NuevoEmail))
            {
                perfilAModificar.tblEmail = perfilModificarDTO.NuevoEmail;
            }

            // Actualizar código secreto si se proporciona uno nuevo
            if (!string.IsNullOrWhiteSpace(perfilModificarDTO.NuevoCodigoSecreto))
            {
                perfilAModificar.tblCodigoSecreto = await PasswordHasher.HashPasswordAsync(perfilModificarDTO.NuevoCodigoSecreto);
            }

            var perfilModificado = await _drivenPerfilModificar.ModificarPerfil(perfilAModificar);

            if (perfilModificado == null)
            {
                throw new Exception("Error al modificar el perfil en el repositorio.");
            }

            var resultDTO = new PerfilRespuestaDTODriver
            {
                Email = perfilModificado.tblEmail,
                FechaCreacion = perfilModificado.tblFechaCreacion,
                Negocios = perfilModificado.tblNegocios,
                Roles = new System.Collections.Generic.List<Domain.Entities.Rol>()
            };

            return resultDTO;
        }
    }
}
