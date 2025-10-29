using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Producto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public string Categoria { get; set; }
        public int PrecioSugerido { get; set; }
        public int Costo { get; set; }  
        public List<Ingrediente> Ingredientes { get; set; }

    }
}
