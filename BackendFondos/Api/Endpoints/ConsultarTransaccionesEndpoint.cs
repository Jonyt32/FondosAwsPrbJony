using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;

public class ConsultarTransaccionesEndpoint : EndpointWithoutRequest<List<TransaccionDto>>
{
    private readonly AutoMapper.IMapper _mapper;
    private readonly ITransaccionService _transService;
    private readonly ILogger<ConsultarTransaccionesEndpoint> _logger;

    public ConsultarTransaccionesEndpoint(AutoMapper.IMapper mapper, ITransaccionService transService, ILogger<ConsultarTransaccionesEndpoint> logger)
    {
        _mapper = mapper;
        _transService = transService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("/transacciones/{id}");
        Roles("User");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var clienteID = Route<string>("id")!;

            var items = await _transService.ObtenerPorClienteAsync(clienteID);
            var resp = _mapper.Map<List<TransaccionDto>>(items);
            await Send.OkAsync(resp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar transacciones");
            await Send.ErrorsAsync();
        }
    }
}
