using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObtenerPorIdAsync(string userId);
        Task<Usuario> ObtenerUsuarioPorEmail(string email);
        Task<Usuario?> LoginAsync(string email, string password);
        Task<Usuario> CrearAsync(Usuario usuario);
        Task ActualizarAsync(Usuario usuario);
        Task EliminarAsync(string userId);
    }
}
