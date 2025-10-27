using BackendFondos.Application.DTOs;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Domain.Validators;

namespace BackendFondos.Domain.Services
{
    public class GestorSuscripcionesService : IGestorSuscripcionesService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IFondoRepository _fondoRepository;
        private readonly ITransaccionRepository _transaccionRepository;
        private readonly SuscripcionValidator _suscripcionValidator;
        private readonly CancelacionValidator _cancelacionValidator;
        private readonly TransaccionValidator _transaccionValidator;
        private readonly INotificacionEmailService _notificacionEmailService;
        private readonly ILogService<GestorSuscripcionesService> _log;

        public GestorSuscripcionesService(
            IClienteRepository clienteRepository,
            IFondoRepository fondoRepository,
            ITransaccionRepository transaccionRepository,
            SuscripcionValidator suscripcionValidator,
            CancelacionValidator cancelacionValidator,
            TransaccionValidator transaccionValidator,
            INotificacionEmailService notificacionEmailService,
            ILogService<GestorSuscripcionesService> log)
        {
            _clienteRepository = clienteRepository;
            _fondoRepository = fondoRepository;
            _transaccionRepository = transaccionRepository;
            _suscripcionValidator = suscripcionValidator;
            _cancelacionValidator = cancelacionValidator;
            _transaccionValidator = transaccionValidator;
            _notificacionEmailService = notificacionEmailService;
            _log = log;
        }

        public async Task<ResultadoOperacionDto> SuscribirClienteAFondoAsync(string clienteId, string fondoId)
        {
            _log.Info($"Iniciando suscripción del cliente {clienteId} al fondo {fondoId}");

            var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);
            if (cliente == null)
                return Error("El cliente no existe", clienteId, fondoId);

            var fondo = await _fondoRepository.ObtenerPorIdAsync(fondoId);
            if (fondo == null)
                return Error("El fondo no existe", clienteId, fondoId);

            var fondosActivos = await _fondoRepository.ObtenerFondosPorIdsAsync(cliente.FondosActivos);

            try
            {
                _suscripcionValidator.Validar(cliente, fondo, fondosActivos);
            }
            catch (InvalidOperationException ex)
            {
                return Error(ex.Message, clienteId, fondoId);
            }

            cliente.FondosActivos.Add(fondoId);
            await _clienteRepository.ActualizarAsync(cliente);
            await CrearTransaccionAsync(clienteId, fondo, TipoTransaccion.Suscripcion);
            await NotificarAsync(cliente, fondo, TipoTransaccion.Suscripcion);

            _log.Info($"Suscripción completada para cliente {clienteId} al fondo {fondoId}");

            return new ResultadoOperacionDto
            {
                Exito = true,
                MensajeNotificacion = $"Suscripción exitosa al fondo {fondo.NombreFondo} por ${fondo.MontoMinimo:N0}",
                ClienteId = clienteId,
                FondoId = fondoId,
                Tipo = TipoTransaccion.Suscripcion
            };
        }

        public async Task<ResultadoOperacionDto> CancelarSuscripcionAsync(string clienteId, string fondoId)
        {
            _log.Info($"Iniciando cancelación de suscripción del cliente {clienteId} al fondo {fondoId}");

            var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);
            if (cliente == null)
                return Error("El cliente no existe", clienteId, fondoId);

            var fondo = await _fondoRepository.ObtenerPorIdAsync(fondoId);
            if (fondo == null)
                return Error("El fondo no existe", clienteId, fondoId);

            try
            {
                _cancelacionValidator.Validar(cliente, fondo);
            }
            catch (InvalidOperationException ex)
            {
                return Error(ex.Message, clienteId, fondoId);
            }

            cliente.FondosActivos.Remove(fondoId);
            await _clienteRepository.ActualizarAsync(cliente);
            await CrearTransaccionAsync(clienteId, fondo, TipoTransaccion.Cancelacion);
            await NotificarAsync(cliente, fondo, TipoTransaccion.Cancelacion);

            _log.Info($"Cancelación completada para cliente {clienteId} al fondo {fondoId}");

            return new ResultadoOperacionDto
            {
                Exito = true,
                MensajeNotificacion = $"Cancelación exitosa del fondo {fondo.NombreFondo}",
                ClienteId = clienteId,
                FondoId = fondoId,
                Tipo = TipoTransaccion.Cancelacion
            };
        }

        private ResultadoOperacionDto Error(string mensaje, string clienteId, string fondoId)
        {
            _log.Warn($"Operación fallida: {mensaje}");
            return new ResultadoOperacionDto
            {
                Exito = false,
                MensajeNotificacion = $"Error de negocio: {mensaje}",
                ClienteId = clienteId,
                FondoId = fondoId
            };
        }

        private async Task CrearTransaccionAsync(string clienteId, Fondo fondo, TipoTransaccion tipo)
        {
            var transaccion = new Transaccion
            {
                ClienteID = clienteId,
                FondoID = fondo.FondoID,
                Tipo = tipo,
                Monto = fondo.MontoMinimo,
                Fecha = DateTime.UtcNow
            };

            _transaccionValidator.Validar(transaccion);
            await _transaccionRepository.CrearAsync(transaccion);
        }

        private async Task NotificarAsync(Cliente cliente, Fondo fondo, TipoTransaccion tipo)
        {
            var mensaje = tipo == TipoTransaccion.Suscripcion
                ? $"Hola {cliente.Nombre},<br/>Te has suscrito exitosamente al fondo <strong>{fondo.NombreFondo}</strong> por ${fondo.MontoMinimo:N0}."
                : $"Hola {cliente.Nombre},<br/>Se ha cancelado tu suscripción al fondo <strong>{fondo.NombreFondo}</strong>.";

            var asunto = tipo == TipoTransaccion.Suscripcion
                ? "Suscripción confirmada"
                : "Cancelación confirmada";

            if (cliente.PreferenciaNotificacion == "email" &&
                cliente.CanalesNotificacion.TryGetValue("email", out var correo))
            {
                await _notificacionEmailService.EnviarCorreoAsync(correo, asunto, mensaje);
            }
            else if (cliente.PreferenciaNotificacion == "sms" &&
                     cliente.CanalesNotificacion.TryGetValue("sms", out var telefono))
            {
                await _notificacionEmailService.EnviarSmsAsync(telefono, mensaje, tipo);
            }
        }
    }
}