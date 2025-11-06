using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Services
{
    public interface IClienteService
    {
        Task CrearClienteAsync(Cliente cliente);
        Task<Cliente> ObtenerClientePorIdAsync(string clienteId);
        Task ActualizarClienteAsync(Cliente cliente);
        Task ActualizarSaldoClienteAsync(string clienteId, decimal monto);
        Task EliminarClienteAsync(string clienteId);
    }
}

