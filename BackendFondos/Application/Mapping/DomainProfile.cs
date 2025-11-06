using AutoMapper;
using BackendFondos.Domain.Entities;
using BackendFondos.Application.DTOs;


namespace BackendFondos.Application.Mapping
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.ClienteID, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));

            CreateMap<Cliente, ClienteDto>();

            CreateMap<FondoDto, Fondo>()
                .ForMember(dest => dest.FondoID, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));

            CreateMap<Fondo, FondoDto>();

            CreateMap<TransaccionDto, Transaccion>()
                .ForMember(dest => dest.TransaccionID, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(_ => DateTime.UtcNow.ToString("o")));
            
            CreateMap<Transaccion, TransaccionDto>();

            CreateMap<UsuarioDto, Usuario>().ForMember(dest => dest.UsuarioID, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));
            CreateMap<Usuario, UsuarioDto>();
        }
    }
}