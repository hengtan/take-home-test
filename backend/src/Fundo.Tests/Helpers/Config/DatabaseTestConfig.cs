using System;
using Microsoft.Extensions.Configuration;

namespace Fundo.Services.Tests.Helpers.Config;

public static class DatabaseTestConfig
{
    private static IConfiguration Configuration { get; }

    static DatabaseTestConfig()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static string ConnectionString => Configuration.GetConnectionString("Default") ?? string.Empty;
}