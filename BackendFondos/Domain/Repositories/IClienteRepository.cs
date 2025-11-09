using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente?>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(string clienteId);
        Task<Cliente?> ObtenerClientePorEmailAsync(string email);
        Task CrearAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(string clienteId);
    }
}