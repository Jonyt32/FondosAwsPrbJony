namespace BackendFondos.Application.DTOs
{
    public class FondoDto
    {
        public string FondoID { get; set; }
        public string NombreFondo { get; set; } = string.Empty;
        public decimal MontoMinimo { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}
