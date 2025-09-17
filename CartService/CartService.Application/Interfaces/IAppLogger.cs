namespace CartService.Application.Interfaces
{
    public interface IAppLogger<T>
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogDebug(string message);
        void LogError(string message, Exception ex);
    }
}
