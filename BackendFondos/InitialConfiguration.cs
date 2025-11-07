using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Services;
using System.Text.Json;

namespace BackendFondos
{
    public static class InitialConfiguration
    {

        private static async Task GenerarUsuarioAdmin(IUsuarioServices usuarioServices, IConfiguration config, AdminCreds cred) 
        {
            var admin = new Usuario() 
            {
                Email = cred.Email,
                NombreUsuario = "JonyT",
                PasswordHash = cred.Password,
                Rol = cred.Rol,
            };

            var existeusuari = await usuarioServices.ObtenerUsuarioPorEmail(admin.Email);
            if (existeusuari == null) 
            {
                await usuarioServices.CrearAsync(admin);
            }
        }

        private static async Task<AdminCreds> ObtenerCredencialesAdminAsync(IConfiguration config)
        {
            var secretId = config["Secrets:Admin"];
            var client = new AmazonSecretsManagerClient();
            var response = await client.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = secretId
            });

            return JsonSerializer.Deserialize<AdminCreds>(response.SecretString)!;
        }

        private static async Task CrearFondosPorDefecto(IFondoService fondoService)
        {
            var existentes = await fondoService.ObtenerTodosFondos();
            if (!existentes.Any())
            {
                await fondoService.CrearFondoAsync(new Fondo { NombreFondo = "FPV_BTG_PACTUAL_RECAUDADORA", Categoria= "FPV", MontoMinimo= 75000 });
                await fondoService.CrearFondoAsync(new Fondo { NombreFondo = "FPV_BTG_PACTUAL_ECOPETROL", Categoria = "FPV", MontoMinimo = 125000 });
                await fondoService.CrearFondoAsync(new Fondo { NombreFondo = "DEUDAPRIVADA", Categoria = "FIC", MontoMinimo = 50000 });
                await fondoService.CrearFondoAsync(new Fondo { NombreFondo = "FDO-ACCIONES", Categoria = "FIC", MontoMinimo = 250000 });
                await fondoService.CrearFondoAsync(new Fondo { NombreFondo = "FPV_BTG_PACTUAL_DINAMICA", Categoria = "FPV", MontoMinimo = 100000 });
            }
        }

        public static async Task ResetDataAsync(IUsuarioServices usuarioService, IFondoService fondoService, IConfiguration config)
        {
            var creds = await ObtenerCredencialesAdminAsync(config);
            // 1. Eliminar fondos
            var fondos = await fondoService.ObtenerTodosFondos();
            foreach (var fondo in fondos)
            {
                await fondoService.EliminarFondoAsync(fondo.FondoID);
            }
            // 2. Eliminar usuarios excepto el admin
            var usuarios = await usuarioService.ObtenerTodosUsuarios();
            foreach (var usuario in usuarios)
            {
                if (!usuario.Email.Equals(creds.Email, StringComparison.OrdinalIgnoreCase))
                {
                    await usuarioService.EliminarAsync(usuario.UsuarioID);
                }
            }
            // 3. Insertar fondos por defecto
            await CrearFondosPorDefecto(fondoService);

            // 4. Insertar admin desde Secrets Manager
            await GenerarUsuarioAdmin(usuarioService, config, creds);
        }


    }

    public class AdminCreds
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Rol { get; set; } = default!;
        public string Key { get; set; } = default!;
    }

}
