using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Services
{
    public interface ITransaccionService
    {
        Task RegistrarTransaccionAsync(Transaccion transaccion);
        Task<List<Transaccion>> ObtenerPorClienteAsync(string clienteId);
    }
}

