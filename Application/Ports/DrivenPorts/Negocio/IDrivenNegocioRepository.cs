using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Negocio
{
    public interface IDrivenNegocioRepository
    {
        Task<List<Domain.Entities.Negocio>> ObtenerNegociosDisponiblesAsync(Guid perfilId);
    }
}
