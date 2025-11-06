using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;

public class FondosAsignadosClienteEndpoint : EndpointWithoutRequest<List<FondoDto>>
{
    private readonly AutoMapper.IMapper _mapper;
    private readonly IClienteService _clienteService;
    private readonly IFondoService _fondoService;


    public FondosAsignadosClienteEndpoint(AutoMapper.IMapper mapper, IClienteService clienteService, IFondoService fondoService )
    {
        _mapper = mapper;
        _clienteService = clienteService;
        _fondoService = fondoService;
    }

    public override void Configure()
    {
        Get("/clientes/{id}/fondos");
        Roles("User");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var clienteId = Route<string>("id");
        try
        {
            var cliente = await _clienteService.ObtenerClientePorIdAsync(clienteId);
            var fondosCliente = await _fondoService.ObtenerFondosPorIdsAsync(cliente.FondosActivos.ToList());
            var resp = _mapper.Map<List<FondoDto>>(fondosCliente);
            await Send.OkAsync(resp);
        }
        catch (Exception)
        {
            await Send.ErrorsAsync();
        }
    }
}