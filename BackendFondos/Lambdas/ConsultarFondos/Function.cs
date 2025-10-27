using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Infrastructure.Repositories;
using System.Text.Json;

namespace BackendFondos.Lambdas.ConsultarFondos
{
    public class Function
    {
        private readonly IFondoRepository _fondoRepository;

        public Function(IFondoRepository fondoRepository)
        {
            _fondoRepository = fondoRepository;
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var fondos = await _fondoRepository.ObtenerTodosAsync();

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(fondos),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}