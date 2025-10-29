using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Negocio
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion{ get; set; }
        public string TipoNegocio { get; set; }
        public List<Producto> Productos { get; set; }
    }
}
