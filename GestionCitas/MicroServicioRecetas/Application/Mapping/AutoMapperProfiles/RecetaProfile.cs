using AutoMapper;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioRecetas.Application.DTOs;
using MicroServicioRecetas.Domain.Models;

namespace MicroServicioRecetas.Application.Mapping.AutoMapperProfiles
{
    public class RecetaProfile : Profile
    {
        public RecetaProfile()
        {
            CreateMap<Receta, RecetaDTO>()
                .ForMember(dest => dest.RecetaId, opt => opt.MapFrom(src => src.RecetaId))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.CitaId, opt => opt.MapFrom(src => src.CitaId))
                .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId))
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ReverseMap(); // Habilitar mapeo en ambas direcciones si es necesario


            CreateMap<CreateRecetaDTO, RecetaDTO>()
                .ForMember(dest => dest.RecetaId, opt => opt.Ignore())
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.CitaId, opt => opt.MapFrom(src => src.CitaId))
                .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId))
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ReverseMap();

            CreateMap<CreateRecetaDTO, Receta>()
            .ForMember(dest => dest.RecetaId, opt => opt.Ignore())
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
            .ForMember(dest => dest.CitaId, opt => opt.MapFrom(src => src.CitaId))
            .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId))
            .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
            .ReverseMap(); // Habilitar mapeo en ambas direcciones si es necesario

            CreateMap<UpdateRecetaDTO, Receta>()
           .ForMember(dest => dest.RecetaId, opt => opt.Ignore())
           .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
           .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
           .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
           .ForMember(dest => dest.CitaId, opt => opt.Ignore())
           .ForMember(dest => dest.MedicoId, opt => opt.Ignore())
           .ForMember(dest => dest.PacienteId, opt => opt.Ignore())
           .ReverseMap(); // Habilitar mapeo en ambas direcciones si es necesario
        }
    }
}
