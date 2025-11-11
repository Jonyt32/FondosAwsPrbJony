using AutoMapper;
using BackendFondos.Domain.Entities;
using BackendFondos.Application.DTOs;


namespace BackendFondos.Application.Mapping
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {

            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.FondosActivos, opt => opt.MapFrom(src => src.FondosActivos != null ? src.FondosActivos.ToList() : new List<string>()))
                .ForMember(dest => dest.SaldoDisponible, opt=> opt.MapFrom(src => src.Saldo != null ? src.Saldo : 0));

            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.FondosActivos, opt => opt.MapFrom(src => src.FondosActivos != null ? src.FondosActivos.ToList() : new List<string>()))
                .ForMember(dest => dest.Saldo, opt => opt.MapFrom(src => src.SaldoDisponible != null ? src.SaldoDisponible : 0));

            CreateMap<FondoDto, Fondo>()
                .ForMember(dest => dest.FondoID, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));

            CreateMap<Fondo, FondoDto>();

            CreateMap<TransaccionDto, Transaccion>()
                .ForMember(dest => dest.TransaccionID, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(_ => DateTime.UtcNow.ToString("o")));
            
            CreateMap<Transaccion, TransaccionDto>();

            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.UsuarioID, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(_ => _.Password));
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(_ => _.PasswordHash));
        }
    }
}