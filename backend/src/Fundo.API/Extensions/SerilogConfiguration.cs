using Serilog;

namespace Fundo.API.Extensions;

public static class SerilogConfiguration
{
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
            .WriteTo.File("logs/fundo-log.json", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}