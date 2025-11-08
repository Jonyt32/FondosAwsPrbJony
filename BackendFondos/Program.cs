using Amazon.DynamoDBv2;
using Amazon.SimpleEmail;
using BackendFondos;
using BackendFondos.Application.Mapping;
using BackendFondos.Domain.Repositories;
using BackendFondos.Domain.Services;
using BackendFondos.Domain.Validators;
using BackendFondos.Infrastructure.Dynamo;
using BackendFondos.Infrastructure.Repositories;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DomainProfile>();
});

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IFondoRepository, FondoRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IGestorSuscripcionesService, GestorSuscripcionesService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();
builder.Services.AddScoped<IFondoService, FondoService>();
builder.Services.AddScoped<IUsuarioServices, UsuarioServices>();
builder.Services.AddScoped<SuscripcionValidator>();
builder.Services.AddScoped<CancelacionValidator>();
builder.Services.AddScoped<TransaccionValidator>();

builder.Services.AddScoped(typeof(ILogService<>), typeof(LogService<>));
builder.Services.AddAWSService<IAmazonSimpleEmailService>();
builder.Services.AddScoped<INotificacionEmailService, NotificacionEmailService>();

builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddSingleton<DynamoDbContext>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "UHJ1ZWJhSkBueVQwcnIzc0FkbWluMTIz");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "fondos-api",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "fondos-app",
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "UHJ1ZWJhSkBueVQwcnIzc0FkbWluMTIz")),
        RoleClaimType = ClaimTypes.Role
    };

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment() || builder.Configuration["App:ResetOnStartup"] == "true")
{
    using var scope = app.Services.CreateScope();
    var usuarioService = scope.ServiceProvider.GetRequiredService<IUsuarioServices>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var fondoService = scope.ServiceProvider.GetRequiredService<IFondoService>();
    await InitialConfiguration.ResetDataAsync(usuarioService, fondoService, builder.Configuration);
}


app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Excepción no manejada en la solicitud");
        throw;
    }
});


app.UseFastEndpoints();
app.UseSwaggerGen();


app.Run();
