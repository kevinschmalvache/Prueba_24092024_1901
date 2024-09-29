using AutoMapper;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioRecetas.Application.DTOs;
using MicroServicioRecetas.Application.Interfaces;
using MicroServicioRecetas.Domain.Interfaces;
using MicroServicioRecetas.Domain.Models;
using MicroServicioRecetas.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioRecetas.Application.Services
{
    /// <summary>
    /// Servicio para la gestión de recetas, que implementa la interfaz IRecetaService.
    /// Utiliza un repositorio de recetas, un servicio de dominio para la lógica de negocio
    /// y AutoMapper para la conversión de entidades a DTOs.
    /// </summary>
    public class RecetaService : IRecetaService
    {
        private readonly IRecetaRepository _recetaRepository;
        private readonly IRecetaDomainService _recetaDomainService;
        private readonly IMapper _mapper;

        public RecetaService(IRecetaRepository recetaRepository, IRecetaDomainService recetaDomainService, IMapper mapper)
        {
            _recetaRepository = recetaRepository;
            _recetaDomainService = recetaDomainService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene una lista de todas las recetas desde el repositorio.
        /// </summary>
        /// <returns>Una lista de objetos RecetaDTO que representan las recetas.</returns>
        public async Task<List<RecetaDTO>> GetRecetas()
        {
            List<Receta> recetas = await _recetaRepository.GetRecetasAsync();
            return _mapper.Map<List<RecetaDTO>>(recetas);
        }

        /// <summary>
        /// Obtiene una receta por su ID desde el repositorio.
        /// Realiza una validación para asegurar que la receta existe.
        /// </summary>
        /// <param name="id">El ID de la receta.</param>
        /// <returns>Un objeto RecetaDTO que representa la receta solicitada.</returns>
        public async Task<RecetaDTO> GetRecetaById(int id)
        {
            Receta receta = await _recetaRepository.GetRecetaByIdAsync(id);
            _recetaDomainService.ExistReceta(receta);
            return _mapper.Map<RecetaDTO>(receta);
        }

        /// <summary>
        /// Agrega una nueva receta al repositorio.
        /// Realiza validaciones de negocio antes de agregar la receta.
        /// </summary>
        /// <param name="recetaDto">Objeto DTO que contiene los datos de la receta a crear.</param>
        /// <returns>Un objeto RecetaDTO que representa la receta creada.</returns>
        public async Task<RecetaDTO> AddReceta(CreateRecetaDTO recetaDto)
        {
            Receta receta = _mapper.Map<Receta>(recetaDto);
            receta.FechaCreacion = DateTime.Now;

            // Validaciones de negocio antes de agregar
            _recetaDomainService.ValidateReceta(receta);

            Receta addedReceta = await _recetaRepository.AddRecetaAsync(receta);
            RecetaDTO resultDto = _mapper.Map<RecetaDTO>(addedReceta);
            return resultDto;
        }

        /// <summary>
        /// Actualiza una receta existente en el repositorio.
        /// </summary>
        /// <param name="id">El ID de la receta a actualizar.</param>
        /// <param name="recetaDto">Objeto DTO que contiene los datos actualizados de la receta.</param>
        /// <returns>Un objeto RecetaDTO que representa la receta actualizada.</returns>
        public async Task<RecetaDTO> UpdateReceta(int id, UpdateRecetaDTO recetaDto)
        {
            Receta receta = _mapper.Map<Receta>(recetaDto);
            Receta updatedReceta = await _recetaRepository.UpdateRecetaAsync(id, receta);

            return _mapper.Map<RecetaDTO>(updatedReceta);
        }

        /// <summary>
        /// Elimina una receta del repositorio por su ID.
        /// </summary>
        /// <param name="id">El ID de la receta a eliminar.</param>
        public async Task DeleteReceta(int id)
        {
            await _recetaRepository.DeleteRecetaAsync(id);
        }

        /// <summary>
        /// Obtiene todas las recetas asociadas a un paciente por su ID.
        /// </summary>
        /// <param name="pacienteId">El ID del paciente.</param>
        /// <returns>Una lista de objetos RecetaDTO que representan las recetas del paciente.</returns>
        public async Task<List<RecetaDTO>> GetRecetasByPacienteId(int pacienteId)
        {
            List<Receta> recetas = await _recetaRepository.GetRecetasByPacienteId(pacienteId);
            return _mapper.Map<List<RecetaDTO>>(recetas);
        }

        /// <summary>
        /// Actualiza el estado de una receta en el repositorio.
        /// Valida el nuevo estado antes de realizar la actualización.
        /// </summary>
        /// <param name="id">El ID de la receta a actualizar.</param>
        /// <param name="nuevoEstado">El nuevo estado de la receta.</param>
        /// <returns>Un valor booleano indicando si la actualización fue exitosa.</returns>
        public async Task<bool> UpdateEstadoReceta(int id, string nuevoEstado)
        {
            var objReceta = new Receta { Estado = nuevoEstado };
            _recetaDomainService.ValidarEstadosReceta(objReceta);
            return await _recetaRepository.UpdateEstadoRecetaAsync(id, nuevoEstado);
        }
    }
}
