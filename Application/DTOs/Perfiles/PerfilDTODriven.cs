using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.DTOs.Perfiles
{
    public class PerfilDTODriven
    {
        public Guid tblIdentidad { get; set; }
        public string tblEmail { get; set; }
        public string tblCodigoSecreto { get; set; }
        public DateTime tblFechaCreacion { get; set; }
        public List<Negocio> tblNegocios { get; set; }
        public List<Guid> tblPermisosRolIds { get; set; }

        public PerfilDTODriven() { }

        public PerfilDTODriven(Guid tblIdentidad, string tblEmail, string tblCodigoSecreto, DateTime tblFechaCreacion, List<Negocio> tblNegocios, List<Guid> tblPermisosRolIds)
        {
            this.tblIdentidad = tblIdentidad;
            this.tblEmail = tblEmail;
            this.tblCodigoSecreto = tblCodigoSecreto;
            this.tblFechaCreacion = tblFechaCreacion;
            this.tblNegocios = tblNegocios;
            this.tblPermisosRolIds = tblPermisosRolIds;
        }
    }
}