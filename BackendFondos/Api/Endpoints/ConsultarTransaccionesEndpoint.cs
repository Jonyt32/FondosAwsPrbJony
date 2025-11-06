using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;

public class ConsultarTransaccionesEndpoint : EndpointWithoutRequest<List<TransaccionDto>>
{
    private readonly AutoMapper.IMapper _mapper;
    private readonly ITransaccionService _transService;

    public ConsultarTransaccionesEndpoint(AutoMapper.IMapper mapper, ITransaccionService transService)
    {
        _mapper = mapper;
        _transService = transService;
    }

    public override void Configure()
    {
        Get("/transacciones/{id}");
        Roles("User");
        AllowAnonymous();
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
        catch (Exception)
        {
            await Send.ErrorsAsync();
        }
    }
}
