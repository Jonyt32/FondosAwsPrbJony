using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;


[Authorize(Policy = "Admin")]
public class CrearFondosEndpoint : Endpoint<FondoDto>
{
    private readonly AutoMapper.IMapper _mapper;
    private readonly ILogger<CrearFondosEndpoint> _logger;

    public override void Configure()
    {
        Post("/fondo-nuevo");
        Roles("Admin");
    }

    public CrearFondosEndpoint(AutoMapper.IMapper mapper, ILogger<CrearFondosEndpoint> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task HandleAsync(FondoDto req, CancellationToken ct)
    {
        try
        {
            var service = Resolve<IFondoService>();
            var fondo = _mapper.Map<Fondo>(req);

            await service.CrearFondoAsync(fondo);
            await Send.OkAsync(new { Message = $"Fondos creado exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear fondo");
            await Send.ErrorsAsync();
        }
    }
}

