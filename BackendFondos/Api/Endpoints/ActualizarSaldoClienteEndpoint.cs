using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;


public class ActualizarSaldoClienteEndpoint : EndpointWithoutRequest
{
    private readonly IClienteService _clienteService;
    private readonly ILogger<ActualizarSaldoClienteEndpoint> _logger;

    public ActualizarSaldoClienteEndpoint(IClienteService clienteService, ILogger<ActualizarSaldoClienteEndpoint> logger    )
    {
        _clienteService = clienteService;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/clientes/actualizar-saldo-cliente/{id}/{saldo}");
        Roles("Admin");
        //AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var clienteID = Route<string>("id")!;
            var saldo = Route<decimal>("saldo")!;

            await _clienteService.ActualizarSaldoClienteAsync(clienteID, saldo);
            
            await Send.OkAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar saldo cliente");
            await Send.ErrorsAsync();
        }
    }
}
