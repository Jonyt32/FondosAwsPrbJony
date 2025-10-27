namespace BackendFondos.Application.DTOs
{
    public class ResultadoOperacionDto
    {
        public bool Exito { get; set; }
        public string MensajeNotificacion { get; set; }
        public string ClienteId { get; set; }
        public string FondoId { get; set; }
        public TipoTransaccion Tipo { get; set; }
    }
}


