using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Services;

namespace BackendFondos
{
    public static class InitialConfiguration
    {

        public static async Task GenerarUsuarioAdmin(IUsuarioServices usuarioServices) 
        {
            var admin = new Usuario() 
            {
                Email = "tonyjorres01@hotmail.com",
                NombreUsuario = "JonyT",
                PasswordHash = "Admin123",
                Rol = "Admin",
            };

            var existeusuari = await usuarioServices.ObtenerUsuarioPorEmail(admin.Email);
            if (existeusuari == null) 
            {
                await usuarioServices.CrearAsync(admin);
            }
        }
    }
}
