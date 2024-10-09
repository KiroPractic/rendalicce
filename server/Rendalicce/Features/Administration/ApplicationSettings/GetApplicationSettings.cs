using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Persistency;

namespace Rendalicce.Features.Administration.ApplicationSettings;

public class GetApplicationSettings
{
    public sealed record GetApplicationSettingsResult(string WebsiteUrl);
    
    public sealed class GetApplicationSettingsEndpoint : EndpointWithoutRequest<GetApplicationSettingsResult>
    {
        public required DatabaseContext DbContext { get; init; }
        
        public override void Configure()
        {
            Get("administration/application-settings");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var applicationSettings = await DbContext.ApplicationSettings
                .Select(a => new GetApplicationSettingsResult(a.WebsiteRootUrl))
                .SingleOrDefaultAsync(ct);
            
            await SendAsync(applicationSettings!, cancellation: ct);
        }
    }
}