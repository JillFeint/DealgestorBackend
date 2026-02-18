using Application.DTOs.Ingredientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports.DrivenPorts.Ingrediente
{
    /// <summary>
    /// Puerto para crear ingredientes en la capa de persistencia.
    /// </summary>
    public interface PortDrivenIngredienteCrear
    {
        /// <summary>
        /// Verifica si existe un ingrediente con el nombre y referencia especificados.
        /// </summary>
        /// <param name="referencia">La referencia del ingrediente.</param>
        /// <param name="nombre">El nombre del ingrediente.</param>
        /// <returns>True si el ingrediente existe, false en caso contrario.</returns>
        Task<bool> ExisteIngredienteNombre(int referencia, string nombre);
        
        /// <summary>
        /// Crea un nuevo ingrediente en la base de datos.
        /// </summary>
        /// <param name="nombreIngrediente">Los datos del ingrediente a crear.</param>
        /// <returns>El ingrediente creado.</returns>
        Task<Domain.Entities.Ingrediente> CrearIngrediente(IngredienteDTODriver nombreIngrediente);
    }
}
