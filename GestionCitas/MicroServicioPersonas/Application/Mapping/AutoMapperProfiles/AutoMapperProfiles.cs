using AutoMapper;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioPersonas.Domain.Models;

namespace MicroServicioPersonas.Application.Mapping.AutoMapperProfiles
{
    public class PersonaProfile : Profile
    {
        public PersonaProfile()
        {
            // Mapeo entre Persona y PersonaDTO
            CreateMap<Persona, PersonaDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
                .ForMember(dest => dest.TipoDePersona, opt => opt.MapFrom(src => src.TipoDePersona))
                .ReverseMap(); // Habilitar mapeo en ambas direcciones si es necesario

            // Mapeo entre CreatePersonaDTO y PersonaDTO (para creación)
            CreateMap<CreatePersonaDTO, PersonaDTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
                .ForMember(dest => dest.TipoDePersona, opt => opt.MapFrom(src => src.TipoDePersona))
                .ReverseMap();

            // Mapeo entre CreatePersonaDTO y Persona
            CreateMap<CreatePersonaDTO, Persona>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
                .ForMember(dest => dest.TipoDePersona, opt => opt.MapFrom(src => src.TipoDePersona))
                .ReverseMap();

            // Mapeo de UpdatePersonaDTO a Persona
            CreateMap<UpdatePersonaDTO, Persona>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
                .ForAllOtherMembers(opt => opt.Ignore()); // Ignorar todos los demás campos
        }
    }
}
