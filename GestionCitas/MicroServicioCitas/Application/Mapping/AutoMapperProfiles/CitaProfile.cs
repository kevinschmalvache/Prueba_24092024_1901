using AutoMapper;
using MicroServicioCitas.Application.DTOs;
using MicroServicioCitas.Domain.Models;

namespace MicroServicioCitas.Application.Mapping.AutoMapperProfiles
{
    public class CitaProfile : Profile
    {
        public CitaProfile()
        {
            // Mapeo entre Cita y CitaDTO
            CreateMap<Cita, CitaDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.Lugar, opt => opt.MapFrom(src => src.Lugar))
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ReverseMap(); // Habilitar mapeo en ambas direcciones si es necesario

            // Mapeo entre CreateCitaDTO y CitaDTO (para creación)
            CreateMap<CreateCitaDTO, CitaDTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.Lugar, opt => opt.MapFrom(src => src.Lugar))
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId))
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ReverseMap();

            // Mapeo entre CreateCitaDTO y Cita
            CreateMap<CreateCitaDTO, Cita>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.Lugar, opt => opt.MapFrom(src => src.Lugar))
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId))
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ReverseMap();

            // Mapeo de UpdateCitaDTO a Cita
            CreateMap<UpdateCitaDTO, Cita>()
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.Lugar, opt => opt.MapFrom(src => src.Lugar))
                //.ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForAllOtherMembers(opt => opt.Ignore()); // Ignorar todos los demás campos
        }
    }
}
