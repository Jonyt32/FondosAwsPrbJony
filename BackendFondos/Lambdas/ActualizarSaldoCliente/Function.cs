using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AutoMapper;
using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using System.Text.Json;

namespace BackendFondos.Lambdas.ActualizarSaldoCliente
{
    public class Function
    {
        private readonly IClienteRepository _clienteRepository;

        public Function(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
           
            var dto = JsonSerializer.Deserialize<ActualizarSaldoRequestDto>(request.Body);
            if (dto == null)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 400,
                    Body = "Formato de solicitud inválido"
                };
            }

            if(string.IsNullOrWhiteSpace(dto.ClienteId) && dto.NuevoSaldo < 0)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 400,
                    Body = "Datos de solicitud inválidos"
                };
            }

            var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);
            if (cliente == null)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 404,
                    Body = $"Cliente con ID {dto.ClienteId} no encontrado"
                };
            }

            cliente.Saldo = dto.NuevoSaldo;
            await _clienteRepository.ActualizarAsync(cliente);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(cliente),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}