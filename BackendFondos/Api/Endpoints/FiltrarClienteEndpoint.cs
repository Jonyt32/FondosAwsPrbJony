using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;

namespace BackendFondos.Api.Endpoints
{
    public class FiltrarClienteEndpoint: EndpointWithoutRequest
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ActualizarSaldoClienteEndpoint> _logger;
        private readonly AutoMapper.IMapper _mapper;

        public FiltrarClienteEndpoint(IClienteService clienteService, AutoMapper.IMapper mapper, ILogger<ActualizarSaldoClienteEndpoint> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        public override void Configure()
        {
            Post("/clientes/filtrar-cliente/{email}");
            Roles("Admin, User");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var email = Route<string>("email")!;

                var lstCliente = await _clienteService.FiltrarClientes(email);
                var resp = _mapper.Map<List<ClienteDto>>(lstCliente);
                await Send.OkAsync(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar saldo cliente");
                await Send.ErrorsAsync();
            }
        }
    }
}
