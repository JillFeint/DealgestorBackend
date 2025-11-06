using Application.DTOs.Roles;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Application.Ports.DrivenPorts.Rol
{
    public interface IDrivenRolRepository
    {
        Task<List<Domain.Entities.Rol>> ConsultarRolAsync(string Nombre);       
    }
}