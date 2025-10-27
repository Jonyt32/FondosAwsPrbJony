using BackendFondos.Application.DTOs;

namespace BackendFondos.Domain.Services
{
    public interface INotificacionEmailService
    {
        Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml);
        Task<ResultadoOperacionDto> EnviarSmsAsync(string numeroTelefono, string mensaje, TipoTransaccion tipo);
    }
}
