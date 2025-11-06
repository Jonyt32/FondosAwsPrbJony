using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;

public class SubscribirClienteAFondoEndpoint : EndpointWithoutRequest<ResultadoOperacionDto>
{
    private readonly IGestorSuscripcionesService _suscripcionService;

    public SubscribirClienteAFondoEndpoint(IGestorSuscripcionesService suscripcionService) => _suscripcionService = suscripcionService;

    public override void Configure()
    {
        Post("/clientes/{id}/{fondoId}/suscribir");
        Roles("User");
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
            await Send.ErrorsAsync();
        }
    }
}