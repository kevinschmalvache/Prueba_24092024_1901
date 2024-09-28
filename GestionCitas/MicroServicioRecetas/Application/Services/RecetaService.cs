using AutoMapper;
using MicroServicioPersonas.Application.DTOs;
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
    public class RecetaService : IRecetaService
    {
        private readonly IRecetaRepository _recetaRepository;
        private readonly RecetaDomainService _recetaDomainService;
        private readonly IMapper _mapper;

        public RecetaService(IRecetaRepository recetaRepository, RecetaDomainService recetaDomainService, IMapper mapper)
        {
            _recetaRepository = recetaRepository;
            _recetaDomainService = recetaDomainService;
            _mapper = mapper;
        }

        public async Task<List<RecetaDTO>> GetRecetas()
        {
            List<Receta> recetas = await _recetaRepository.GetRecetasAsync();
            return _mapper.Map<List<RecetaDTO>>(recetas);
        }

        public async Task<RecetaDTO> GetRecetaById(int id)
        {
            Receta receta = await _recetaRepository.GetRecetaByIdAsync(id);
            _recetaDomainService.ExistReceta(receta);
            return _mapper.Map<RecetaDTO>(receta);
        }

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

        public async Task<RecetaDTO> UpdateReceta(int id, UpdateRecetaDTO recetaDto)
        {
            Receta receta = _mapper.Map<Receta>(recetaDto);
            Receta updatedReceta = await _recetaRepository.UpdateRecetaAsync(id, receta);

            return _mapper.Map<RecetaDTO>(updatedReceta);
        }

        public async Task DeleteReceta(int id)
        {
            await _recetaRepository.DeleteRecetaAsync(id);
        }

        public async Task<List<RecetaDTO>> GetRecetasByPacienteId(int pacienteId)
        {
            List<Receta> recetas = await _recetaRepository.GetRecetasByPacienteId(pacienteId);
            return _mapper.Map<List<RecetaDTO>>(recetas);
        }

        public async Task<bool> UpdateEstadoReceta(int id, string nuevoEstado)
        {
            var objReceta = new Receta { Estado = nuevoEstado };
            _recetaDomainService.ValidarEstadosReceta(objReceta);
            return await _recetaRepository.UpdateEstadoRecetaAsync(id, nuevoEstado);
        }
    }
}
