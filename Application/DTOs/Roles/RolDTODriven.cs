using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Roles
{
    public class RolDTODriven
    {
        public Guid tblIdentificacion { get; set; }
        public string tblTipo { get; set; }
        public string tblNombre { get; set; }

        public RolDTODriven() { }

        public RolDTODriven(Guid tblIdentificacion, string tblTipo, string tblNombre)
        {
            this.tblIdentificacion = tblIdentificacion;
            this.tblTipo = tblTipo;
            this.tblNombre = tblNombre;
        }
    }
}