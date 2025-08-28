
using Microsoft.Extensions.Logging;

namespace BD2Tools.Utils;

public abstract class LoggedService<T>
{
    protected ILogger<T> Logger { get; }

    protected LoggedService(ILogger<T> logger)
    {
        Logger = logger;
    }
}
