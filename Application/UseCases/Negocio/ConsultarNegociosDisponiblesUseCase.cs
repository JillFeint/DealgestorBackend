using Application.Ports.DriverPorts.Perfil;
using Application.Ports.DrivenPorts.Negocio;
using Application.ViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.Negocio
{
    // 💡 Implementa: El puerto de entrada.
    public class ConsultarNegociosDisponiblesUseCase : IDriverPerfilPort
    {
        // 💡 Inyección: Inyectamos el puerto de salida (Driven Port).
        private readonly IDrivenNegocioRepository _negocioRepository;

        public ConsultarNegociosDisponiblesUseCase(IDrivenNegocioRepository negocioRepository)
        {
            _negocioRepository = negocioRepository;
        }

        // Implementación del método definido en IDriverPerfilPort
        public async Task<List<NegocioViewModel>> ObtenerNegociosDisponiblesParaPerfil(Guid perfilId)
        {
            // 1. Lógica de Negocio: Llamar al puerto de salida.
            // El Caso de Uso NO sabe que el repositorio llama a una API externa.
            var negociosDominio = await _negocioRepository.ObtenerNegociosDisponiblesAsync(perfilId);

            // 2. Mapeo a ViewModel (Buena Práctica: Aislar la capa de Dominio)
            return negociosDominio.Select(n => new NegocioViewModel
            {
                Id = n.Id,
                Referencia = n.Referencia,
                Nombre = n.Nombre,
                TipoNegocio = n.TipoNegocio,
                // Agrega más propiedades necesarias para la exposición
            }).ToList();
        }
    }}