using System;
using Microsoft.Extensions.Configuration;

namespace Fundo.Services.Tests.Helpers.Config;

public static class JwtTestConfig
{
    private static readonly IConfigurationRoot _config;

    static JwtTestConfig()
    {
        _config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();
    }

    public static string Key => _config["Jwt:Key"]!;
    public static string Issuer => _config["Jwt:Issuer"]!;
    public static string Audience => _config["Jwt:Audience"]!;
}