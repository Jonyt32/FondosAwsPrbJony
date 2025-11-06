using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Services
{
    public interface IFondoService
    {
        Task<List<Fondo>> ObtenerTodosFondos();
        Task CrearFondoAsync(Fondo fondo);
        Task<Fondo> ObtenerFondoPorIdAsync(string fondoId);
        Task<List<Fondo>> ObtenerFondosPorIdsAsync(List<string> fondos);
        Task ActualizarFondoAsync(Fondo fondo);
        Task EliminarFondoAsync(string fondoId);
    }
}

