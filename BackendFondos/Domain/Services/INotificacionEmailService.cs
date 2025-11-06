using BackendFondos.Application.DTOs;

namespace BackendFondos.Domain.Services
{
    public interface INotificacionEmailService
    {
        Task<ResultadoOperacionDto> EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml, TipoTransaccion tipo);
        Task<ResultadoOperacionDto> EnviarSmsAsync(string numeroTelefono, string mensaje, TipoTransaccion tipo);
    }
}
