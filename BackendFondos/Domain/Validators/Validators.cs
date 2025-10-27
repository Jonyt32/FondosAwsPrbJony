
using BackendFondos.Domain.Entities;

namespace BackendFondos.Domain.Validators
{
    public class SuscripcionValidator
    {
        public void Validar(Cliente cliente, Fondo fondo, IEnumerable<Fondo> fondosActivos)
        {
            if (cliente.FondosActivos.Contains(fondo.FondoID))
                throw new InvalidOperationException("El cliente ya está suscrito a este fondo");

            var saldoDisponible = cliente.CalcularSaldoDisponible(fondosActivos);

            if (saldoDisponible < fondo.MontoMinimo)
                throw new InvalidOperationException($"Saldo insuficiente. Disponible: ${saldoDisponible:N0}, requerido: ${fondo.MontoMinimo:N0}");

            

        }
    }

    public class CancelacionValidator
    {
        public void Validar(Cliente cliente, Fondo fondo)
        {
            if (!cliente.FondosActivos.Contains(fondo.FondoID))
                throw new InvalidOperationException("El cliente no está suscrito a este fondo");

            if (fondo.MontoMinimo <= 0)
                throw new InvalidOperationException("El fondo no tiene un monto válido para reembolsar");
        }
    }

    public class TransaccionValidator
    {
        public void Validar(Transaccion transaccion)
        {
            if (transaccion.Monto <= 0)
                throw new InvalidOperationException("El monto de la transacción debe ser mayor a cero");

            if (transaccion.Fecha == default)
                throw new InvalidOperationException("La fecha de la transacción no es válida");

            if (!Enum.IsDefined(typeof(TipoTransaccion), transaccion.Tipo))
                throw new InvalidOperationException("Tipo de transacción inválido");
        }
    }


}
