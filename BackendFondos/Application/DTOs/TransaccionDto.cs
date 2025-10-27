namespace BackendFondos.Application.DTOs
{
    public class TransaccionDto
    {
        public string Tipo { get; set; } = string.Empty;
        public string FondoID { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Notificacion { get; set; } = "email";
    }
}