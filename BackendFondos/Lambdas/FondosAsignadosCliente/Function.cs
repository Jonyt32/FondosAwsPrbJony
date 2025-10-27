using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using System.Text.Json;

namespace BackendFondos.Lambdas.FondosAsignadosCliente
{
    public class Function
    {
        private readonly IClienteRepository _clienteRepo;

        public Function(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var cliente = JsonSerializer.Deserialize<Cliente>(request.Body);
            await _clienteRepo.CrearAsync(cliente);

            return new APIGatewayProxyResponse
            {
                StatusCode = 201,
                Body = JsonSerializer.Serialize(new { mensaje = "Cliente creado exitosamente", clienteId = cliente.ClienteID }),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}