namespace App
{

public class FileLogger : ILogger, IDisposable
{
    string filePath;
    static object _lock = new object();

    public FileLogger(string path) {
        filePath = path;
    }

    public IDisposable BeginScope<TState>(TState state) {
        return this;
    }

    public void Dispose() { }

    public bool IsEnabled(LogLevel logLevel) {
        //return logLevel == LogLevel.Trace;
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) {
        lock (_lock)
        {
            File.AppendAllText(
                filePath,
                formatter(state, exception) + Environment.NewLine);
        }
    }
}

public class FileLoggerProvider : ILoggerProvider {
   string path;

   public FileLoggerProvider(string path) {
       this.path = path;
   }

   public ILogger CreateLogger(string categoryName) {
       return new FileLogger(path);
   }

   public void Dispose() {}
}

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
    {
        builder.AddProvider(new FileLoggerProvider(filePath));
        return builder;
    }
}

class Application
{
    static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        // встановлюємо файл для логування
        builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
        // налаштування логування з допомогою властивістю Logging йде до
        // створення об'єкта WebApplication
        var app = builder.Build();

        app.Run(async (context) =>
        {
            app.Logger.LogInformation(
                $"Path: {context.Request.Path}  Time:{DateTime.Now.ToLongTimeString()}");

            await context.Response.WriteAsync("Hello World!");
        });

        app.Run();
    }
}

}