using Microsoft.Extensions.Logging;

namespace DianaLab.Core.Utils;

public abstract class LoggedService<T>
{
    protected ILogger<T> Logger { get; }

    protected LoggedService(ILogger<T> logger)
    {
        Logger = logger;
    }
}
