namespace BackendFondos.Domain.Services
{
    public interface ILogService<T>

    {
        void Info(string mensaje);
        void Warn(string mensaje);
        void Error(string mensaje, Exception ex = null);
    }
}

