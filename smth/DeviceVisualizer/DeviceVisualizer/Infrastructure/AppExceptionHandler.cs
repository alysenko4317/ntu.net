using System;
using System.Diagnostics;

namespace DeviceVisualizer.Infrastructure;

public sealed class AppExceptionHandler : IObserver<Exception>
{
    private readonly ILogger _logger;

    public AppExceptionHandler(ILogger logger)
    {
        _logger = logger;
    }

    public void OnCompleted()
    {
        if (Debugger.IsAttached) Debugger.Break();
    }

    public void OnError(Exception error)
    {
        if (Debugger.IsAttached) Debugger.Break();
        _logger.Error(error.Source ?? "Unhandled", error);
    }

    public void OnNext(Exception value)
    {
        if (Debugger.IsAttached) Debugger.Break();
        _logger.Error(value.Source ?? "Unhandled", value);
    }
}