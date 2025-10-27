
using Amazon.DynamoDBv2;
using BackendFondos.Domain.Repositories;
using BackendFondos.Domain.Services;
using BackendFondos.Domain.Validators;
using BackendFondos.Infrastructure.Dynamo;
using BackendFondos.Infrastructure.Repositories;

namespace BackendFondos.Lambdas
{
    public static class LambdaHostBuilder
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IFondoRepository, FondoRepository>();
            services.AddScoped<ITransaccionRepository, TransaccionRepository>();
            services.AddScoped<INotificacionEmailService, NotificacionEmailService>();
            services.AddScoped(typeof(ILogService<>), typeof(LogService<>));

            services.AddScoped<SuscripcionValidator>();
            services.AddScoped<CancelacionValidator>();
            services.AddScoped<TransaccionValidator>();
            services.AddScoped<IGestorSuscripcionesService, GestorSuscripcionesService>();

            services.AddAWSService<IAmazonDynamoDB>();
            services.AddSingleton<DynamoDbContext>();
                

        }
    }
}
