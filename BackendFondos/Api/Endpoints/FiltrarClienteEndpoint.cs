using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using FastEndpoints;
using System.Collections.Generic;

namespace BackendFondos.Api.Endpoints
{
    public class FiltrarClienteEndpoint: EndpointWithoutRequest
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<FiltrarClienteEndpoint> _logger;
        

        public FiltrarClienteEndpoint(IClienteService clienteService, ILogger<FiltrarClienteEndpoint> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        public override void Configure()
        {
            Get("/clientes/filtrar-cliente/{email}");
            Roles("Admin, User");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var email = Route<string>("email");
                var dto = new List<ClienteDto>();
                var lstCliente = await _clienteService.FiltrarClientes(email);

                if (lstCliente == null || lstCliente.Count == 0)
                {
                    await Send.OkAsync(dto);
                    return;
                }

                try
                {
                    dto = lstCliente.Select(x => new ClienteDto() 
                    {
                        ClienteID = x.ClienteID,
                        Email = x.Email,
                        FondosActivos = (x.FondosActivos != null) ? x.FondosActivos.ToList(): new List<string>(),
                        Nombre = x.Nombre,
                        SaldoDisponible = x.Saldo,
                        PreferenciaNotificacion = x.PreferenciaNotificacion
                    }).ToList();
                    
                    await Send.OkAsync(dto);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al mapear Cliente → ClienteDto");
                    await Send.ErrorsAsync();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar saldo cliente");
                await Send.ErrorsAsync();
            }
        }
    }
}
