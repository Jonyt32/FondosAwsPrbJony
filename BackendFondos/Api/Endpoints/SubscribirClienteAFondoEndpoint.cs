using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;

public class SubscribirClienteAFondoEndpoint : EndpointWithoutRequest<ResultadoOperacionDto>
{
    private readonly IGestorSuscripcionesService _suscripcionService;
    private readonly ILogger<SubscribirClienteAFondoEndpoint> _logger;

    public SubscribirClienteAFondoEndpoint(IGestorSuscripcionesService suscripcionService, ILogger<SubscribirClienteAFondoEndpoint> logger) 
    {
        _suscripcionService = suscripcionService;
        _logger = logger;
    } 

    public override void Configure()
    {
        Post("/clientes/{id}/{fondoId}/suscribir");
        Roles("Admin, User");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var clienteId = Route<string>("id");
        var fondoId = Route<string>("fondoId");

        if (string.IsNullOrWhiteSpace(clienteId) || string.IsNullOrWhiteSpace(fondoId))
        {
            await Send.ErrorsAsync();
            return;
        }

        try
        {
            var response = await _suscripcionService.SuscribirClienteAFondoAsync(clienteId, fondoId);
            if (response == null)
            {
                await Send.ErrorsAsync();
                return;
            }

            await Send.OkAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al suscribir fondo {fondoId} al cliente {clienteId}");
            await Send.ErrorsAsync();
        }
    }
}