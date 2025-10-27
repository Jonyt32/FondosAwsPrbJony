using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Repositories;
using AutoMapper;
using System.Net;


namespace BackendFondos.Lambdas.ConsultarTransacciones
{
    public class Function
    {
        private readonly ITransaccionRepository _transaccionRepo;
        private readonly IMapper _mapper;

        public Function(ITransaccionRepository transaccionRepo, IMapper mapper)
        {
            _transaccionRepo = transaccionRepo;
            _mapper = mapper;
        }

        public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            if (!request.QueryStringParameters.TryGetValue("clienteId", out var clienteId) || string.IsNullOrWhiteSpace(clienteId))
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = "Parámetro 'clienteId' es obligatorio"
                };
            }

            var transacciones = await _transaccionRepo.ObtenerPorClienteAsync(clienteId);
            var dtoList = _mapper.Map<List<TransaccionDto>>(transacciones);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = System.Text.Json.JsonSerializer.Serialize(dtoList),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}