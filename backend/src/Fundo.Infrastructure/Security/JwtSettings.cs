namespace Fundo.Infrastructure.Security;

public record JwtSettings
{
    public string Key { get; init; } = default!;
    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public int ExpirationMinutes { get; init; } = default!;
    public Dictionary<string, string> Clients { get; init; } = default!;
}