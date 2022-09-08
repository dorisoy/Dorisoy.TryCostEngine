﻿namespace TryCostEngine.MAUI.Services
{
    using System;
    using TryCostEngine.Core;
    using Microsoft.Extensions.Logging;
    using Microsoft.JSInterop;

    // This is a workaround for the currently missing global exception handling mechanism in Blazor. If the user code generates
    // an assembly that makes the app throw an exception, we need to override the stored assembly in browser's cache storage
    // so the app works on reload in cases of exceptions for duplicate routes, invalid directives, etc.
    // (Approach: https://github.com/dotnet/aspnetcore/issues/13452#issuecomment-632660280)
    public class HandleCriticalUserComponentExceptionsLogger : ILogger
    {

        public HandleCriticalUserComponentExceptionsLogger()
        {

        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (exception?.ToString()?.Contains(CompilationService.DefaultRootNamespace) ?? false)
            {

            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel.Critical;

        public IDisposable BeginScope<TState>(TState state) => NoOpDisposable.Instance;

        private class NoOpDisposable : IDisposable
        {
            public static readonly NoOpDisposable Instance = new();

            public void Dispose()
            {
            }
        }
    }
}
