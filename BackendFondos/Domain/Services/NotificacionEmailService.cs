namespace BackendFondos.Domain.Services
{
    using Amazon.SimpleEmail;
    using Amazon.SimpleEmail.Model;
    using BackendFondos.Application.DTOs;

    public class NotificacionEmailService : INotificacionEmailService
    {
        private readonly IAmazonSimpleEmailService _ses;
        private readonly string _remitente;

        public NotificacionEmailService(IAmazonSimpleEmailService ses, IConfiguration config)
        {
            _ses = ses;
            _remitente = config["Ses:Remitente"];
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            var request = new SendEmailRequest
            {
                Source = _remitente,
                Destination = new Destination { ToAddresses = new List<string> { destinatario } },
                Message = new Message
                {
                    Subject = new Content(asunto),
                    Body = new Body
                    {
                        Html = new Content { Charset = "UTF-8", Data = cuerpoHtml }
                    }
                }
            };

            await _ses.SendEmailAsync(request);
        }

        public async Task<ResultadoOperacionDto> EnviarSmsAsync(string numeroTelefono, string mensaje, TipoTransaccion tipo)
        {
            // Simulación del envío
            Console.WriteLine($"[SMS Simulado] Enviado a: {numeroTelefono} | Mensaje: {mensaje}");

            return new ResultadoOperacionDto
            {
                Exito = true,
                MensajeNotificacion = $"SMS enviado a {numeroTelefono}",
                Tipo = tipo
            };
        }


    }
}