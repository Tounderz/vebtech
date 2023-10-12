using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using vebtech.Application.Services.Interfaces;
using vebtech.Domain.Models.Configurations;

namespace vebtech.Application.Services;

public class JwtService : IJwtService
{
    private readonly JwtConfig _jwtConfig;

    public JwtService(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public string GenerateJwt(string email)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimsIdentity.DefaultNameClaimType, email),
    };

        ClaimsIdentity claimsIdentity =
        new(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        var now = DateTime.UtcNow;
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken
            (
        issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(_jwtConfig.LifeTime)),
                signingCredentials: credentials
            );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}
