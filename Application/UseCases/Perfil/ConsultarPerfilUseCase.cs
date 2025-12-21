using Application.DTOs.Perfiles;
using Application.Ports.DrivenPorts.Perfil;
using Application.Ports.DriverPorts.Perfil;
using Application.Services;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Perfil
{
    public class ConsultarPerfilUseCase : PortDriverPerfilConsultar
    {
        private readonly PortDrivenPerfilConsultar _perfilDrivenConsultar;

        public ConsultarPerfilUseCase(PortDrivenPerfilConsultar perfilDrivenConsultar)
        {
            _perfilDrivenConsultar = perfilDrivenConsultar ?? throw new ArgumentNullException(nameof(perfilDrivenConsultar));
        }

        public async Task<PerfilRespuestaDTODriver> ConsultarPerfilPorEmail(string email, string codeEspecial)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.", nameof(email));

            if (string.IsNullOrWhiteSpace(codeEspecial))
                throw new ArgumentException("El código especial no puede estar vacío.", nameof(codeEspecial));

            Domain.Entities.Perfil perfilDominio = await _perfilDrivenConsultar.ConsultarPerfilPorEmailAsync(email);

            if (perfilDominio == null)
            {
                throw new ArgumentException("No se encontró ningún perfil con el email especificado.", nameof(email));
            }

            bool codigoValido = await PasswordHasher.VerifyPasswordAsync(codeEspecial, perfilDominio.CodigoSecreto);
            
            if (!codigoValido)
            {
                throw new UnauthorizedAccessException("El código especial es incorrecto.");
            }

            return new PerfilRespuestaDTODriver
            {           
                Email = perfilDominio.Email,
                Negocios = perfilDominio.Negocios,
                Roles = perfilDominio.PermisosRol,
                FechaCreacion = perfilDominio.FechaCreacion
            };
        }
    }
}