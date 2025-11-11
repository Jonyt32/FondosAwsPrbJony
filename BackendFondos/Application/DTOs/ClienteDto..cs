namespace BackendFondos.Application.DTOs
{
    public class ClienteDto
    {
        public decimal? SaldoDisponible { get; set; }
        public List<string>? FondosActivos { get; set; }
        public string? PreferenciaNotificacion { get; set; }
        public string? Nombre { get; set; }
        public string? ClienteID { get; set; }
        public string? Email { get; set; }

    }
}