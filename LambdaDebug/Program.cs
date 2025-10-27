


using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using BackendFondos.Infrastructure.Dynamo;
using BackendFondos.Infrastructure.Repositories;
using BackendFondos.Lambdas.ConsultarFondos;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

// 🔧 Crear cliente DynamoDB real
var awsOptions = config.GetAWSOptions(); // Si tenés configurado el perfil AWS
var dynamoDbClient = awsOptions.CreateServiceClient<IAmazonDynamoDB>();

// 🔧 Crear contexto DynamoDB
var dbContext = new DynamoDbContext();

// 🔧 Instanciar repositorio real
var fondoRepo = new FondoRepository(dbContext, dynamoDbClient, config);

// 🔧 Instanciar Lambda real
var lambda = new Function(fondoRepo);

// 🔧 Simular request de API Gateway
var request = new APIGatewayProxyRequest
{
    QueryStringParameters = new Dictionary<string, string>() // Si usás filtros
};

// 🔧 Ejecutar handler
var response = await lambda.FunctionHandler(request, null);

// 🔧 Mostrar resultado
Console.WriteLine("StatusCode: " + response.StatusCode);
Console.WriteLine("Body: " + response.Body);