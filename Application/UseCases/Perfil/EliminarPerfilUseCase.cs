using Application.Ports.DrivenPorts.Perfil;
using Application.Ports.DriverPorts.Perfil;
using Application.Services;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Perfil
{
    public class EliminarPerfilUseCase : PortDriverPerfilEliminar
    {
        private readonly PortDrivenPerfilEliminar _perfilDrivenEliminar;

        public EliminarPerfilUseCase(PortDrivenPerfilEliminar perfilDrivenEliminar)
        {
            _perfilDrivenEliminar = perfilDrivenEliminar ?? throw new ArgumentNullException(nameof(perfilDrivenEliminar));
        }

        public async Task<bool> EliminarPerfil(string email, string codeEspecial)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.", nameof(email));

            if (string.IsNullOrWhiteSpace(codeEspecial))
                throw new ArgumentException("El código especial no puede estar vacío.");

            Domain.Entities.Perfil perfilDominio = await _perfilDrivenEliminar.ObtenerPerfilPorEmail(email);

            if (perfilDominio == null)
            {
                throw new ArgumentException("No se encontró ningún perfil con el email especificado.", nameof(email));
            }

            bool codigoValido = await PasswordHasher.VerifyPasswordAsync(codeEspecial, perfilDominio.CodigoSecreto);
            
            if (!codigoValido)
            {
                throw new UnauthorizedAccessException("El código especial es incorrecto.");
            }

            return await _perfilDrivenEliminar.EliminarPerfilPorEmail(email);
        }
    }
}
