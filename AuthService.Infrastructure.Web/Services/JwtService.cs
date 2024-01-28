using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AuthService.Application.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Web.Services;

public class JwtService : IJwtService
{
    private readonly string _issuer;
    private readonly string[] _audience;
    private readonly string _secretKey;
    private readonly int _lifetime;
    private readonly string[] _claims;
    private readonly IUserClaimsPrincipalFactory<UserData> _principalFactory;

    public JwtService(string issuer, string[] audience, string secretKey, int lifetime, string[] claims,
        IUserClaimsPrincipalFactory<UserData> principalFactory)
    {
        _issuer = issuer;
        _audience = audience;
        _secretKey = secretKey;
        _lifetime = lifetime;
        _claims = claims;
        _principalFactory = principalFactory;
    }

    public async Task<string> GenerateJwt(UserData user)
    {
        var principal = await _principalFactory.CreateAsync(user);

        var claims = principal.Claims.Where(claim => _claims.Contains(claim.Type)).ToArray();

        var now = DateTime.Now;
        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: $"[{string.Join(", ", _audience)}]",
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromMinutes(_lifetime)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}