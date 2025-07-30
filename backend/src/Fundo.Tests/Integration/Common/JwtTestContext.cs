using System.Net.Http;
using System.Net.Http.Headers;
using Fundo.Infrastructure.Security;
using Fundo.Services.Tests.Helpers;

namespace Fundo.Services.Tests.Integration.Common;

public static class JwtTestContext
{
    private static readonly JwtSettings _settings = new()
    {
        Key = "super-strong-secret-key-for-testing-123!",
        Issuer = "FundoIssuer",
        Audience = "FundoAudience"
    };

    private static string GetToken()
    {
        return JwtTokenGenerator.GenerateToken(_settings);
    }

    public static void AttachToken(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", GetToken());
    }

    public static JwtSettings Settings => _settings;
}