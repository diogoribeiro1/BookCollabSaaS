using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookCollabSaaS.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookCollabSaaS.Infrastructure.Auth;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        var keyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing"));
        _key = new SymmetricSecurityKey(keyBytes);
        _issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing");
        _audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing");
    }

    public string GenerateToken(string userId, string role)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));
        if (string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException(nameof(role));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
