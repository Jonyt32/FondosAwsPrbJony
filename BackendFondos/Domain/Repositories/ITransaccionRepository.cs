using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Repositories
{
    public interface ITransaccionRepository
    {
        Task<IEnumerable<Transaccion>> ObtenerPorClienteAsync(string clienteId);
        Task<Transaccion?> ObtenerPorIdAsync(string clienteId, string transaccionId);
        Task CrearAsync(Transaccion transaccion);
        Task EliminarAsync(string clienteId, string transaccionId);
    }
}