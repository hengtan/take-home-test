namespace Fundo.Application.DTOs;

public class ClientAuthRequest
{
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
}