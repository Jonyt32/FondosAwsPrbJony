
namespace BackendFondos.Domain.Services
{
    public class LogService<T> : ILogService<T>
    {
        private readonly ILogger<T> _logger;

        public LogService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void Info(string mensaje) => _logger.LogInformation(mensaje);
        public void Warn(string mensaje) => _logger.LogWarning(mensaje);
        public void Error(string mensaje, Exception ex = null) =>
            _logger.LogError(ex, mensaje);
    }
}
