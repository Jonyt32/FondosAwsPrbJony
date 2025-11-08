using BackendFondos.Domain.Services;
using BackendFondos.Lambdas.CancelarSuscripcion;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;


public class CancelarSuscripcionEndpoint : Endpoint<CancelacionRequestDto>
{
    private readonly IGestorSuscripcionesService _service;
    private readonly ILogger<CancelarSuscripcionEndpoint> _logger;

    public CancelarSuscripcionEndpoint(IGestorSuscripcionesService service, ILogger<CancelarSuscripcionEndpoint> logger) 
    {
        _service = service;
        _logger = logger;
        
    }

    public override void Configure()
    {
        Post("/clientes/cancelar-suscripcion");
        Roles("User");
    }

    public override async Task HandleAsync(CancelacionRequestDto req, CancellationToken ct)
    {
        
        try
        {
            var success = await _service.CancelarSuscripcionAsync(req.ClienteId, req.FondoId);
            if (success == null)
                await Send.ErrorsAsync();
            else
                await Send.OkAsync(success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cancelar suscripcion");
            await Send.ErrorsAsync();
        }
    }
}
