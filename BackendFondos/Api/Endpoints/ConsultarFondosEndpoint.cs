using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;


public class ConsultarFondosEndpoint : EndpointWithoutRequest<List<FondoDto>>
{
    private readonly ILogger<ConsultarFondosEndpoint> _logger;
    private readonly AutoMapper.IMapper _mapper;
    private readonly IFondoService _fondoService;

    public ConsultarFondosEndpoint(AutoMapper.IMapper mapper, IFondoService fondoService, ILogger<ConsultarFondosEndpoint> logger)
    {
        _mapper = mapper;
        _fondoService = fondoService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("/fondos");
        Roles("Admin");
        //AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var fondos = await _fondoService.ObtenerTodosFondos();
            var resp = _mapper.Map<List<FondoDto>>(fondos);
            await Send.OkAsync(resp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar fondos");

            await Send.ErrorsAsync();
        }
    }
}