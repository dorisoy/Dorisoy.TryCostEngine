namespace TryCostEngine.MAUI.Services
{
    using Microsoft.Extensions.Logging;
    using Microsoft.JSInterop;

    public class HandleCriticalUserComponentExceptionsLoggerProvider : ILoggerProvider
    {

        public HandleCriticalUserComponentExceptionsLoggerProvider()
        {
        }

        public ILogger CreateLogger(string categoryName) => new HandleCriticalUserComponentExceptionsLogger();

        public void Dispose()
        {
        }
    }
}
