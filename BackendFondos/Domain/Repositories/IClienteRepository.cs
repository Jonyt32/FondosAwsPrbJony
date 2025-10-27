using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Repositories
{
    public interface IClienteRepository
    {
        Task<Cliente?> ObtenerPorIdAsync(string clienteId);
        Task CrearAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(string clienteId);
    }
}