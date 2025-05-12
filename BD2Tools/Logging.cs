using Microsoft.Extensions.Logging;

public abstract class LoggedService<T>
{
    protected ILogger<T> Logger { get; }

    public LoggedService(ILogger<T> logger)
    {
        Logger = logger;
    }
}