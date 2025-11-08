using Amazon.DynamoDBv2.Model;
using BackendFondos.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendFondos.Api.Endpoints
{
    public class LoginEndpoint : Endpoint<AuthRequest, AuthResponse>
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginEndpoint> _logger;
        public LoginEndpoint(IUsuarioServices usuarioServices, IConfiguration configuration, ILogger<LoginEndpoint> logger)
        {
            _usuarioServices = usuarioServices;
            _configuration = configuration;
            _logger = logger;
        }

        public override void Configure()
        {
            Post("/auth/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(AuthRequest req, CancellationToken ct)
        {
            try
            {
                var usuario = await _usuarioServices.LoginAsync(req.Username, req.Password);
                if (usuario == null) { await Send.UnauthorizedAsync(); return; }

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioID),
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                };

                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
                var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
                var expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiresMinutes"] ?? "60"));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme),
                    Expires = expires,
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwt = tokenHandler.WriteToken(token);
                var roles = new string[]
                {
                    usuario.Rol
                };
                await Send.OkAsync(new AuthResponse { Token = jwt, Expires = expires, Roles = roles });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Login");
                await Send.ErrorsAsync();
            }
            
        }
    }
}

public class AuthRequest
{
    [Required(ErrorMessage = "UserName es obligatorio")]
    public string Username { get; set; } = string.Empty;
    [Required(ErrorMessage ="Password es obligatorio")]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string[] Roles { get; set; } = Array.Empty<string>();
}
