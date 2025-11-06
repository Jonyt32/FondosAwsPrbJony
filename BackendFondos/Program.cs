using Amazon.DynamoDBv2;
using Amazon.SimpleEmail;
using BackendFondos.Application.Mapping;
using BackendFondos.Domain.Repositories;
using BackendFondos.Domain.Services;
using BackendFondos.Domain.Validators;
using BackendFondos.Infrastructure.Dynamo;
using BackendFondos.Infrastructure.Repositories;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using YamlDotNet.Core.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

//builder.Services.AddAutoMapper(typeof(DomainProfile));
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DomainProfile>();
    // cfg.AddProfiles(typeof(OtherProfile).Assembly); // si necesitas más assemblies
});

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IFondoRepository, FondoRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();

builder.Services.AddScoped<IGestorSuscripcionesService, GestorSuscripcionesService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();
builder.Services.AddScoped<IFondoService, FondoService>();
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

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "dG9rZW49QWRtaW4xMjM=");
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "miIssuer",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "miAudience",
        IssuerSigningKey = new SymmetricSecurityKey(key),
        RoleClaimType = "roles" 
    };
    /*options.Events = new JwtBearerEvents
    {
        OnTokenValidated = ctx =>
        {
            // ejemplo para mover claim "roles" a ClaimTypes.Role si es necesario
            var claimsIdentity = ctx.Principal?.Identity as ClaimsIdentity;
            var rolesClaim = claimsIdentity?.FindFirst("roles");
            if (rolesClaim != null && rolesClaim.Value.StartsWith("["))
            {
                // opcional: parsear JSON array si viene como string
            }
            return Task.CompletedTask;
        }
    };*/
});

// autorización
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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");




app.UseAuthentication();
app.UseAuthorization();


app.UseFastEndpoints();
app.UseSwaggerGen();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
