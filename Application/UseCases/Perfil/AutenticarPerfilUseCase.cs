using Application.DTOs.Perfiles;
using Application.Ports.DrivenPorts.Perfil;
using Application.Ports.DrivenPorts.Tarjeta;
using Application.Ports.DriverPorts.Perfil;
using Application.Services;

namespace Application.UseCases.Perfil
{
    /// <summary>
    /// Caso de uso para autenticar un perfil de usuario.
    /// Valida las credenciales, obtiene los roles y genera un token JWT.
    /// </summary>
    public class AutenticarPerfilUseCase : PortDriverPerfilAutenticar
    {
        private readonly PortDrivenPerfilConsultar _perfilDrivenConsultar;
        private readonly IGeneradorDeTokens _generadorDeTokens;

        public AutenticarPerfilUseCase(
            PortDrivenPerfilConsultar perfilDrivenConsultar,
            IGeneradorDeTokens generadorDeTokens)
        {
            _perfilDrivenConsultar = perfilDrivenConsultar ?? throw new ArgumentNullException(nameof(perfilDrivenConsultar));
            _generadorDeTokens = generadorDeTokens ?? throw new ArgumentNullException(nameof(generadorDeTokens));
        }

        public async Task<LoginResponseDTO> AutenticarPerfil(LoginRequestDTO credenciales)
        {
            ValidarCredenciales(credenciales);

            Domain.Entities.Perfil perfilDominio = await _perfilDrivenConsultar.ConsultarPerfilPorEmailAsync(credenciales.Email);

            if (perfilDominio == null)
            {
                throw new ArgumentException($"No existe un perfil con el email '{credenciales.Email}'.");
            }

            bool passwordValido = await PasswordHasher.VerifyPasswordAsync(
                credenciales.CodigoSecreto, 
                perfilDominio.CodigoSecreto);

            if (!passwordValido)
            {
                throw new UnauthorizedAccessException("La contraseña es incorrecta.");
            }

            // El adaptador retorna tanto el token como la fecha de expiración (respeta arquitectura hexagonal)
            var (token, fechaExpiracion) = _generadorDeTokens.CrearTarjetaAcceso(perfilDominio);

            return new LoginResponseDTO
            {
                Token = token,
                Email = perfilDominio.Email,
                Roles = perfilDominio.PermisosRol,
                FechaExpiracion = fechaExpiracion
            };
        }

        private void ValidarCredenciales(LoginRequestDTO credenciales)
        {
            if (credenciales == null)
                throw new ArgumentNullException(nameof(credenciales), "Las credenciales no pueden ser nulas.");

            if (string.IsNullOrWhiteSpace(credenciales.Email))
                throw new ArgumentException("El email no puede estar vacío.", nameof(credenciales.Email));

            if (string.IsNullOrWhiteSpace(credenciales.CodigoSecreto))
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(credenciales.CodigoSecreto));
        }
    }
}
