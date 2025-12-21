using Application.DTOs.Perfiles;
using Application.Ports.DrivenPorts.Perfil;
using Application.Ports.DriverPorts.Perfil;
using Application.Services;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Perfil
{
    public class CrearPerfilUseCase : PortDriverPerfilCrear
    {
        private readonly PortDrivenPerfilCrear _perfilDrivenCrear;

        public CrearPerfilUseCase(PortDrivenPerfilCrear perfilDrivenCrear)
        {
            _perfilDrivenCrear = perfilDrivenCrear ?? throw new ArgumentNullException(nameof(perfilDrivenCrear));
        }

        public async Task<PerfilRespuestaDTODriver> CrearNuevoPerfil(PerfilIngresoDTODriver nuevoPerfilDTO)
        {
            ValidarEntrada(nuevoPerfilDTO);

            bool perfilExiste = await _perfilDrivenCrear.ExistePerfilPorEmail(nuevoPerfilDTO.Email);
            if (perfilExiste)
            {
                throw new ArgumentException($"El email '{nuevoPerfilDTO.Email}' ya está registrado.");
            }

            string codigoSecretoHasheado = await PasswordHasher.HashPasswordAsync(nuevoPerfilDTO.CodigoSecreto);

            var perfilParaGuardar = new PerfilIngresoDTODriver
            {
                Email = nuevoPerfilDTO.Email,
                CodigoSecreto = codigoSecretoHasheado,
            };


            Domain.Entities.Perfil perfilCreado = await _perfilDrivenCrear.CrearPerfil(perfilParaGuardar);

            return new PerfilRespuestaDTODriver
            {
                Email = perfilCreado.Email,
                FechaCreacion = perfilCreado.FechaCreacion
            };
        }

        private void ValidarEntrada(PerfilIngresoDTODriver perfil)
        {
            if (perfil == null)
                throw new ArgumentNullException(nameof(perfil), "El perfil no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(perfil.Email))
                throw new ArgumentException("El email no puede estar vacío.", nameof(perfil.Email));

            if (!EsEmailValido(perfil.Email))
                throw new ArgumentException("El formato del email no es válido.", nameof(perfil.Email));

            if (string.IsNullOrWhiteSpace(perfil.CodigoSecreto))
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(perfil.CodigoSecreto));

            if (perfil.CodigoSecreto.Length < 7)                    
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.", nameof(perfil.CodigoSecreto));
        }

        private bool EsEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}