using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Perfil
    {
        public Guid Identidad { get; set; }
        public string Email { get; set; }
        public string CodigoSecreto { get; set; }
        public List<Negocio> Negocios { get; set; }
        public Rol PermisosRol { get; set; }

    }
}
