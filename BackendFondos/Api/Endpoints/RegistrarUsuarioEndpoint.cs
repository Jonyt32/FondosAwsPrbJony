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
        private readonly ILogger<RegistrarUsuarioEndpoint> _logger;

        public RegistrarUsuarioEndpoint(AutoMapper.IMapper mapper, IUsuarioServices usuarioServices, ILogger<RegistrarUsuarioEndpoint> logger)
        {
            _mapper = mapper;
            _usuarioServices = usuarioServices;
            _logger = logger;
        }
        public override void Configure()
        {
            Post("/Usuarios/register");
            Roles("Admin");
        }

        public override async Task HandleAsync(UsuarioDto req, CancellationToken ct)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(req);
                await _usuarioServices.CrearAsync(usuario);
                await Send.OkAsync(new { message = "Cliente creado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuarios");
                await Send.ErrorsAsync();
            }
        }
    }
}
