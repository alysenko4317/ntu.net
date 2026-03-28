using System;
using ReactiveUI;

namespace DeviceVisualizer.Infrastructure;

public interface ILogger
{
    void Info(string message);
    void Error(string message, Exception? exception = null);
}

public sealed class AppLogger : ILogger
{
    public void Info(string message)
    {
        MessageBus.Current.SendMessage(new ApplicationLog(message));
    }

    public void Error(string message, Exception? exception = null)
    {
        MessageBus.Current.SendMessage(new ApplicationLog($"{message}{(exception is null ? string.Empty : " | " + exception.Message)}"));
    }
}

public sealed record ApplicationLog(string Message);