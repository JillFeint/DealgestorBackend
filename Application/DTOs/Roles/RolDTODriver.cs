using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Roles
{
    public class RolDTODriver
    {
        // 🚨 OJO: Si el campo "identificacion" es generado por el Core, no debería
        // estar en el DTO de creación, sino solo en el JSON de entrada si se
        // requiere que el cliente lo provea. Asumiremos que el cliente lo envía.

        // Si la identificación se autogenera en el Core/Domain, podrías omitir esta propiedad.
        public Guid Identificacion { get; set; } // Mapea "identificacion"
        public string Tipo { get; set; } // Mapea "tipo"
        public string Nombre { get; set; } // Mapea "nombre"
    }
}