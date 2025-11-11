using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Services;
using FastEndpoints;

namespace BackendFondos.Api.Endpoints
{
    public class ConsultarUsuariosEndpoint: EndpointWithoutRequest<List<UsuarioDto>>
    {
        private readonly AutoMapper.IMapper _mapper;
        private readonly IUsuarioServices _usuarioServices;
        private readonly ILogger<ConsultarUsuariosEndpoint> _logger;
        public ConsultarUsuariosEndpoint(AutoMapper.IMapper mapper, IUsuarioServices usuarioServices, ILogger<ConsultarUsuariosEndpoint> logger)
        {
            _mapper = mapper;
            _usuarioServices = usuarioServices;
            _logger = logger;
        }
        public override void Configure()
        {
            Get("/Usuarios/Listar-usuarios");
            Roles("Admin");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var lstUsuario = await _usuarioServices.ObtenerTodosUsuarios();
                var resp = _mapper.Map<List<UsuarioDto>>(lstUsuario);
                await Send.OkAsync(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuarios");
                await Send.ErrorsAsync();
            }
        }
    }
}
