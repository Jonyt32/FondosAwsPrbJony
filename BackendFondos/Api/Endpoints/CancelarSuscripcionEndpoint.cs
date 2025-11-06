using BackendFondos.Domain.Services;
using BackendFondos.Lambdas.CancelarSuscripcion;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;


public class CancelarSuscripcionEndpoint : Endpoint<CancelacionRequestDto>
{
    private readonly IGestorSuscripcionesService _service;

    public CancelarSuscripcionEndpoint(IGestorSuscripcionesService service) => _service = service;

    public override void Configure()
    {
        Post("/clientes/cancelar-suscripcion");
        Roles("User");
        AllowAnonymous();
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
        catch (Exception)
        {
            await Send.ErrorsAsync();
        }
    }
}
