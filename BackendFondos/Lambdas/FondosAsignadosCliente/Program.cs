using BackendFondos.Domain.Repositories;
using BackendFondos.Domain.Services;
using BackendFondos.Domain.Validators;
using BackendFondos.Infrastructure.Repositories;

namespace BackendFondos.Lambdas.FondosAsignadosCliente
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    LambdaHostBuilder.ConfigureServices(services);
                });

            builder.Build().Run();
        }
    }
}