using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AutoMapper;
using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using System.Text.Json;

namespace BackendFondos.Lambdas.CrearCliente
{
    public class Function
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public Function(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var dto = JsonSerializer.Deserialize<ClienteDto>(request.Body);

            if (dto == null)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 400,
                    Body = "Datos inválidos"
                };
            }

            var cliente = _mapper.Map<Cliente>(dto);
            await _clienteRepository.CrearAsync(cliente);

            return new APIGatewayProxyResponse
            {
                StatusCode = 201,
                Body = JsonSerializer.Serialize(cliente),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}