using Microsoft.Extensions.Logging;

namespace DianaLab.GUI.Logging
{
    public static class Log
    {
        public static ILoggerFactory LoggerFactory { get; set; }

        public static ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();

        public static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);
    }
}
