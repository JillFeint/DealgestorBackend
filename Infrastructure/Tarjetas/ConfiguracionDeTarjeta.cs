using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tarjetas
{
    public class ConfiguracionDeTarjeta
    {       
        public const string NombreSeccion = "ConfiguracionDeTarjeta";
        public string Secret { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int DuracionEnMinutos { get; init; } = 60;
    }
}