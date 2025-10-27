using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Repositories
{
    public interface IFondoRepository
    {
        Task<IEnumerable<Fondo>> ObtenerTodosAsync();
        Task<Fondo?> ObtenerPorIdAsync(string fondoId);
        Task CrearAsync(Fondo fondo);
        Task ActualizarAsync(Fondo fondo);
        Task EliminarAsync(string fondoId);
        Task<IEnumerable<Fondo>> ObtenerFondosPorIdsAsync(IEnumerable<string> ids);
    }
}