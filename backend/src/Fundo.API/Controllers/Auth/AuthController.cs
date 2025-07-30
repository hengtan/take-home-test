using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fundo.Application.DTOs;
using Fundo.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fundo.API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IOptions<JwtSettings> jwtOptions) : ControllerBase
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult Token([FromBody] ClientAuthRequest request)
    {
        if (!TryValidateClient(request.ClientId, request.ClientSecret))
            return Unauthorized("Invalid client credentials");

        if (!AreJwtSettingsValid(out var validationError))
            return StatusCode(StatusCodes.Status500InternalServerError, validationError);

        var token = GenerateJwtToken(request.ClientId);

        return Ok(new TokenResponse
        {
            AccessToken = token,
            ExpiresIn = _jwtSettings.ExpirationMinutes * 60
        });
    }

    private bool TryValidateClient(string clientId, string clientSecret)
    {
        return _jwtSettings.Clients.TryGetValue(clientId, out var expectedSecret)
               && expectedSecret == clientSecret;
    }

    private bool AreJwtSettingsValid(out string error)
    {
        if (string.IsNullOrWhiteSpace(_jwtSettings.Key))
        {
            error = "JWT Key is not configured.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(_jwtSettings.Issuer))
        {
            error = "JWT Issuer is not configured.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(_jwtSettings.Audience))
        {
            error = "JWT Audience is not configured.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    private string GenerateJwtToken(string clientId)
    {
        var claims = new[]
        {
            new Claim("client_id", clientId)
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    private class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}