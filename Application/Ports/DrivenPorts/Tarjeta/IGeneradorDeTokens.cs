using Domain.Entities;

namespace Application.Ports.DrivenPorts.Tarjeta
{
    /// <summary>
    /// Puerto conducido para generación de tokens JWT.
    /// Define el contrato para crear tokens de acceso.
    /// </summary>
    public interface IGeneradorDeTokens
    {
        /// <summary>
        /// Crea un token JWT de acceso para un perfil autenticado.
        /// </summary>
        /// <param name="perfil">Perfil del usuario con roles cargados</param>
        /// <returns>Tupla con el token JWT y su fecha de expiración (UTC)</returns>
        (string Token, DateTime FechaExpiracion) CrearTarjetaAcceso(Domain.Entities.Perfil perfil);
    }
}
