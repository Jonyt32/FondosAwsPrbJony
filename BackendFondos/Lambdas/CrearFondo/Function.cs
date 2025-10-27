using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AutoMapper;
using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using System.Text.Json;

namespace BackendFondos.Lambdas.CrearFondo
{
    public class Function
    {
        private readonly IFondoRepository _fondoRepository;
        private readonly IMapper _mapper;

        public Function(IFondoRepository fondoRepository, IMapper mapper)
        {
            _fondoRepository = fondoRepository;
            _mapper = mapper;
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var dto = JsonSerializer.Deserialize<FondoDto>(request.Body);

                if (dto == null)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 400,
                        Body = "Datos inválidos"
                    };
                }

                var fondo = _mapper.Map<Fondo>(dto);
                await _fondoRepository.CrearAsync(fondo);

                return new APIGatewayProxyResponse
                {
                    StatusCode = 201,
                    Body = JsonSerializer.Serialize(fondo),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };

            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Error al crear el fondo: {ex.Message}"
                };
            }
        }
    }
}