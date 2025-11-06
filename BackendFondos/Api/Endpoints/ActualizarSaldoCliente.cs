using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;


public class ActualizarSaldoClienteEndpoint : EndpointWithoutRequest
{
    private readonly IClienteService _clienteService;

    public ActualizarSaldoClienteEndpoint(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    public override void Configure()
    {
        Post("/actualizar-saldo-cliente/{id}/{saldo}");
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
        catch (Exception)
        {
            await Send.ErrorsAsync();
        }
    }
}
