using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Services;
using FastEndpoints;

namespace BackendFondos.Api.Endpoints
{
    public class RegistrarUsuarioEndpoint: Endpoint<UsuarioDto>
    {
        private readonly AutoMapper.IMapper _mapper;
        private readonly IUsuarioServices _usuarioServices;

        public RegistrarUsuarioEndpoint(AutoMapper.IMapper mapper, IUsuarioServices usuarioServices)
        {
            _mapper = mapper;
            _usuarioServices = usuarioServices;
        }
        public override void Configure()
        {
            Post("/Usuarios/register");
            Roles("Admin");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UsuarioDto req, CancellationToken ct)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(req);
                await _usuarioServices.CrearAsync(usuario);
                await Send.OkAsync(new { message = "Cliente creado exitosamente" });
            }
            catch (Exception)
            {
                await Send.ErrorsAsync();
            }
        }
    }
}
