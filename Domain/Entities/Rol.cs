using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rol
    {
        public Guid Identificacion { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }

        public Rol() { }   
        
        public Rol(Guid Identificacion, string Tipo, string Nombre)
        {
            this.Identificacion = Identificacion;
            this.Tipo = Tipo;
            this.Nombre = Nombre;
        }
    }
}
