using BackendFondos.Application.DTOs;

namespace BackendFondos.Domain.Services
{
    public interface IGestorSuscripcionesService
    {

        Task<ResultadoOperacionDto> SuscribirClienteAFondoAsync(string clienteId, string fondoId);
        Task<ResultadoOperacionDto> CancelarSuscripcionAsync(string clienteId, string fondoId);

    }
}


