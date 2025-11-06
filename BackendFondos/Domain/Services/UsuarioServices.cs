using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;

namespace BackendFondos.Domain.Services
{
    public class UsuarioServices: IUsuarioServices
    {
        private readonly IUsuarioRepository _usuariosRepository;
        private readonly INotificacionEmailService _notificacionEmailService;
        public UsuarioServices(IUsuarioRepository usuariosRepository, INotificacionEmailService notificacionEmailService) 
        {
            _usuariosRepository = usuariosRepository;
            _notificacionEmailService = notificacionEmailService;
        }
        public async Task<Usuario> ObtenerPorIdAsync(string userId) 
        {
            try
            {
                return await _usuariosRepository.ObtenerPorIdAsync(userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }
        public async Task<Usuario?> LoginAsync(string email, string password) 
        {
            try
            {
                return await _usuariosRepository.LoginAsync(email, password);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<Usuario?> ObtenerUsuarioPorEmail(string email)
        {
            try
            {
                return await _usuariosRepository.ObtenerUsuarioPorEmail(email);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task CrearAsync(Usuario usuario) 
        {
            try
            {
                var newUsuario = await _usuariosRepository.CrearAsync(usuario);
                string body = $"Bienvenido a nuestro portal de fondos {usuario.Email} " +
                    "ya puedes logearte y hacer uso de nuestros servicios";
                await _notificacionEmailService.EnviarCorreoAsync(newUsuario.Email, "Creación Usuario Fondos Bank", body, TipoTransaccion.CreacionUsuario);
                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task ActualizarAsync(Usuario usuario) 
        {
            try
            {
                await _usuariosRepository.ActualizarAsync(usuario);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task EliminarAsync(string userId) 
        {
            try
            {
                await _usuariosRepository.EliminarAsync(userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
