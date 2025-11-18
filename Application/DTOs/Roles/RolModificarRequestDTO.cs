using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Roles
{
    public class RolModificarRequestDTO
    {
        public string NombreActual { get; set; }
        public string TipoActual { get; set; }
        public string NuevoNombre { get; set; }
        public string NuevoTipo { get; set; }
    }
}
