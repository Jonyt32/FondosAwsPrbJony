using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Services
{
    public interface IFondoService
    {
        Task CrearFondoAsync(Fondo fondo);
        Task<Fondo> ObtenerFondoPorIdAsync(string fondoId);
        Task ActualizarFondoAsync(Fondo fondo);
        Task EliminarFondoAsync(string fondoId);
    }
}

