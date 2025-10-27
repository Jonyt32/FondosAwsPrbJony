namespace BackendFondos.Application.DTOs
{
    public class FondoDto
    {
        public string NombreFondo { get; set; } = string.Empty;
        public decimal MontoMinimo { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}
