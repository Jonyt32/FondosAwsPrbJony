using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Services;
using FastEndpoints;

namespace BackendFondos.Api.Endpoints
{
    public class ConsultarClienteEndpoint: EndpointWithoutRequest<ClienteDto>
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ConsultarClienteEndpoint> _logger;
        private readonly AutoMapper.IMapper _mapper;
        public ConsultarClienteEndpoint(IClienteService clienteService, AutoMapper.IMapper mapper, ILogger<ConsultarClienteEndpoint> logger) 
        {
            _clienteService = clienteService;
            _logger = logger;
            _mapper = mapper;
        }

        public override void Configure()
        {
            Get("/clientes/consultar-cliente/{clientId}");
            Roles("Admin, User");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var clientID = Route<string>("clientId");
                if (string.IsNullOrWhiteSpace(clientID)) 
                {
                    throw new InvalidOperationException("ClienteId no debe ser vacio");
                }
                var cliente = await _clienteService.ObtenerClientePorIdAsync(clientID);
                var resp = _mapper.Map<ClienteDto>(cliente);
                await Send.OkAsync(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente");
                await Send.ErrorsAsync();
            }
        }
    }
}
