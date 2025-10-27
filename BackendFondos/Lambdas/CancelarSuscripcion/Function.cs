using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Services;
using System.Text.Json;

namespace BackendFondos.Lambdas.CancelarSuscripcion
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
            var input = JsonSerializer.Deserialize<CancelacionRequestDto>(request.Body);
            var resultado = await _gestor.CancelarSuscripcionAsync(input.ClienteId, input.FondoId);

            return new APIGatewayProxyResponse
            {
                StatusCode = resultado.Exito ? 200 : 400,
                Body = JsonSerializer.Serialize(resultado),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }

    public class CancelacionRequestDto
    {
        public string ClienteId { get; set; }
        public string FondoId { get; set; }
    }
}