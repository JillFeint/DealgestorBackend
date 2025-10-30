using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Ingrediente
    {
        public Guid Id { get; set; }
        public int Referencia { get; set; }
        public string NombreIngrediente { get; set; }
        public int Cantidad { get; set; }
        public string PrecioPaquete { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
