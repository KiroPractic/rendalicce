using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Rendalicce.Infrastructure.Authentication;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        var authenticatedUser = context.GetHttpContext().GetAuthenticatedUserOrNull();

        // TODO-FK: Reinforce when super administrator is added.
        return authenticatedUser is not null;
    }
}