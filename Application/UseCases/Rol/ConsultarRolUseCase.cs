using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;


namespace Application.UseCases.Rol
{
    public class ConsultarRolUseCase : IDriverRolPort
    {
        private readonly IDrivenRolRepository _rolRepository;

        public ConsultarRolUseCase(IDrivenRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<List<RolDTODriven>> ConsultarIdentificadoresRol(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

            var rolDominio = await _rolRepository.ConsultarRolAsync(nombre);

            return rolDominio.Select(entidadDominio => new RolDTODriven
            {               
                tblIdentificacion = entidadDominio.Identificacion,
                tblTipo = entidadDominio.Tipo,
                tblNombre = entidadDominio.Nombre,
            }).ToList();
        }
    }
}