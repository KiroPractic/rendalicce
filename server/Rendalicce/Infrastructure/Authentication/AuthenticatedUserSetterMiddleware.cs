using System.Security.Claims;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Users;
using Rendalicce.Persistency;

namespace Rendalicce.Infrastructure.Authentication;

public sealed class AuthenticatedUserSetterMiddleware
{
    private readonly RequestDelegate _next;
    
    public AuthenticatedUserSetterMiddleware(RequestDelegate next) => _next = next;
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (Guid.TryParse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
        {
            var dbContext = httpContext.Resolve<DatabaseContext>();
            
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user is not null)
            {
                httpContext.Items.Remove(nameof(User));
                httpContext.Items.Add(nameof(User), user);
            }
        }
        
        await _next(httpContext);
    }
}

public static class AuthenticatedUserSetterMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticatedUserSetter(this IApplicationBuilder builder) => builder.UseMiddleware<AuthenticatedUserSetterMiddleware>();
}
