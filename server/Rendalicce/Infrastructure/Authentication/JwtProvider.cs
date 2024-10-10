using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastEndpoints.Security;
using Microsoft.IdentityModel.Tokens;
using Rendalicce.Configurations;
using Rendalicce.Domain.Users;

namespace Rendalicce.Infrastructure.Authentication;

public sealed class JwtProvider
{
    private readonly JwtConfiguration _jwtConfiguration;

    public JwtProvider(JwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
    }

    public string GenerateJwtToken(User user, bool rememberMe = false)
    {
        return JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = _jwtConfiguration.SigningKey;
                o.ExpireAt = DateTime.Now.AddHours(rememberMe ? 240 : 24);
                o.User.Claims.Add(
                    new Claim[]
                    {
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new(nameof(user.Id), user.Id.ToString()),
                        new(nameof(user.FirstName), user.FirstName),
                        new(nameof(user.LastName), user.LastName),
                        new(nameof(user.Email), user.Email)
                    }
                );
            }
        );
    }

    public string GeneratePasswordResetToken(Guid userId)
    {
        return JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = _jwtConfiguration.SigningKey;
                o.ExpireAt = DateTime.Now.AddMinutes(10);
                o.User.Claims.Add(new Claim(nameof(User.Id), userId.ToString()));
            }
        );
    }

    public Guid? GetUserIdFromPasswordResetToken(string token)
    {
        try
        {
            var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidAudience = null,
                    ValidateIssuer = false,
                    ValidIssuer = null,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SigningKey))
                },
                out _
            );

            if (!Guid.TryParse(claimsPrincipal!.FindFirst(nameof(User.Id))?.Value, out var userId))
                return null;

            return userId;
        }
        catch
        {
            return null;
        }
    }
}