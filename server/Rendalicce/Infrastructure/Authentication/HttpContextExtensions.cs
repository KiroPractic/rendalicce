using Rendalicce.Domain.Users;

namespace Rendalicce.Infrastructure.Authentication;

public static class HttpContextExtensions
{
    public static User? GetAuthenticatedUserOrNull(this HttpContext httpContext)
    {
        return (User?)httpContext.Items[nameof(User)];
    }

    public static User GetAuthenticatedUser(this HttpContext httpContext)
        => GetAuthenticatedUserOrNull(httpContext)!;
}