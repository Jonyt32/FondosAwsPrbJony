using Amazon.Lambda.AspNetCoreServer;

namespace BackendFondos.Lambdas.ConsultarTransacciones
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                // Ya configurado en Program.cs
            });
        }
    }
}