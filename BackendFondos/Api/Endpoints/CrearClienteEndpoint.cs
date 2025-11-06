using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Net;

public class CrearClienteEndpoint : Endpoint<ClienteDto>
{
    private readonly AutoMapper.IMapper _mapper;
    private readonly IClienteService _clienteService;

    public CrearClienteEndpoint(AutoMapper.IMapper mapper, IClienteService clienteService)
    {
        _mapper = mapper;
        _clienteService = clienteService;
    }

    public override void Configure()
    {
        Post("/clientes");
        Roles("Admin");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ClienteDto req, CancellationToken ct)
    {
        try
        {
            var cliente = _mapper.Map<Cliente>(req);
            await _clienteService.CrearClienteAsync(cliente);
            await Send.OkAsync(new { message = "Cliente creado exitosamente" });
        }
        catch (Exception)
        {
            await Send.ErrorsAsync();
        }
    }
}