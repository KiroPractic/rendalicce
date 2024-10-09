using System.Security.Claims;
using FastEndpoints.Security;
using Rendalicce.Configurations;
using Rendalicce.Domain.Users;

namespace Rendalicce.Infrastructure.Authentication;

public sealed class AuthenticationProvider
{
    private readonly JwtConfiguration _jwtConfiguration;

    public AuthenticationProvider(JwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration =  jwtConfiguration;
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
}