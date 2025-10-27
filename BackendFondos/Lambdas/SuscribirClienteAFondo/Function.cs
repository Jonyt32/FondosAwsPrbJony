using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace BackendFondos.Lambdas.SuscribirClienteAFondo
{
    public class Function
    {
        private readonly IGestorSuscripcionesService _gestor;


        public Function(IGestorSuscripcionesService gestor)
        {
            _gestor = gestor;
        }


        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var input = JsonSerializer.Deserialize<SuscripcionRequestDto>(request.Body);

            var resultado = await _gestor.SuscribirClienteAFondoAsync(input.ClienteId, input.FondoId);

            return new APIGatewayProxyResponse
            {
                StatusCode = resultado.Exito ? 200 : 400,
                Body = JsonSerializer.Serialize(resultado),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }

    public class SuscripcionRequestDto
    {
        public string ClienteId { get; set; }
        public string FondoId { get; set; }
    }
}