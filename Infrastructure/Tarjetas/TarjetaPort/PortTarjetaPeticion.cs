using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tarjetas.TarjetaPort
{
    public interface PortTarjetaGenerador
    {
        // El Perfil es una entidad de Dominio, no hay dependencias externas aquí.
        string CrearTarjetaAcceso(Perfil perfil);
    }
}
