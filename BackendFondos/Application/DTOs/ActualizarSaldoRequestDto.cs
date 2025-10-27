namespace BackendFondos.Application.DTOs
{
    public class ActualizarSaldoRequestDto
    {
        public string ClienteId { get; set; }
        public decimal NuevoSaldo { get; set; }
    }
}

