using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace BackendFondos.Domain.Services
{
    public class FondoService : IFondoService
    {
        private readonly IFondoRepository _fondoRepo;

        public FondoService(IFondoRepository fondoRepo)
        {
            _fondoRepo = fondoRepo;
        }

        public async Task<List<Fondo>> ObtenerTodosFondos() 
        {
            var lst = await _fondoRepo.ObtenerTodosAsync();
            if(lst == null)
                throw new InvalidOperationException("No existen fondos");

            return lst.ToList();
        }       

        public async Task CrearFondoAsync(Fondo fondo)
        {
            fondo.FondoID ??= Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(fondo.FondoID))
                throw new InvalidOperationException("El ID del fondo es obligatorio");

            if (fondo.MontoMinimo <= 0)
                throw new InvalidOperationException("El monto mínimo debe ser mayor a cero");

            var existente = await _fondoRepo.ObtenerPorIdAsync(fondo.FondoID);
            if (existente != null)
                throw new InvalidOperationException("El fondo ya existe");

            await _fondoRepo.CrearAsync(fondo);
        }

        public async Task<Fondo> ObtenerFondoPorIdAsync(string fondoId)
        {
            var fondo = await _fondoRepo.ObtenerPorIdAsync(fondoId);
            return fondo ?? throw new InvalidOperationException("Fondo no encontrado");
        }

        public async Task ActualizarFondoAsync(Fondo fondo)
        {
            var existente = await _fondoRepo.ObtenerPorIdAsync(fondo.FondoID);
            if (existente == null)
                throw new InvalidOperationException("Fondo no existe");

            await _fondoRepo.ActualizarAsync(fondo);
        }

        public async Task EliminarFondoAsync(string fondoId)
        {
            var existente = await _fondoRepo.ObtenerPorIdAsync(fondoId);
            if (existente == null)
                throw new InvalidOperationException("Fondo no existe");

            await _fondoRepo.EliminarAsync(fondoId);
        }

        public async Task<List<Fondo>> ObtenerFondosPorIdsAsync(List<string> fondos) 
        {
            var fondosCliente = await _fondoRepo.ObtenerFondosPorIdsAsync(fondos);
            if (fondosCliente == null)
                throw new InvalidOperationException("Cliente no tiene fondos");

            return fondosCliente.ToList();
        }
    }
}



