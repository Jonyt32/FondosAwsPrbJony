namespace BackendFondos.Application.DTOs
{
    public class ClienteDto
    {
        public decimal SaldoDisponible { get; set; }
        public List<string> FondosActivos { get; set; } = new();
        public string PreferenciaNotificacion { get; set; } = "email";
        public string Nombre { get; set; }

    }
}