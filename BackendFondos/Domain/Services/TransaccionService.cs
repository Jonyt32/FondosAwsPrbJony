using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendFondos.Domain.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _transaccionRepo;
        private readonly ILogService<TransaccionService> _log;


        public TransaccionService(ITransaccionRepository transaccionRepo, ILogService<TransaccionService> log)
        {
            _transaccionRepo = transaccionRepo;
            _log = log;
        }

        public async Task RegistrarTransaccionAsync(Transaccion transaccion)
        {
            try
            {
                _log.Info($"Registrando transacción: Cliente={transaccion.ClienteID}, Fondo={transaccion.FondoID}, Tipo={transaccion.Tipo}, Monto={transaccion.Monto}");

                transaccion.TransaccionID ??= Guid.NewGuid().ToString();
                transaccion.Fecha = DateTime.UtcNow;

                if (string.IsNullOrWhiteSpace(transaccion.ClienteID) ||
                    string.IsNullOrWhiteSpace(transaccion.FondoID))
                    throw new InvalidOperationException("Datos incompletos para registrar la transacción");

                await _transaccionRepo.CrearAsync(transaccion);
                _log.Info($"Transacción registrada exitosamente para Cliente={transaccion.ClienteID}");

            }
            catch (System.Exception ex)
            {
                _log.Error($"Error al registrar transacción para Cliente={transaccion.ClienteID}, Fondo={transaccion.FondoID}", ex);
                throw;
            }

        }

        public async Task<List<Transaccion>> ObtenerPorClienteAsync(string clienteId)
        {
            return (await _transaccionRepo.ObtenerPorClienteAsync(clienteId)).ToList();
        }
    }
}

