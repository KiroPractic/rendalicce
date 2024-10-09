using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Persistency;

namespace Rendalicce.Features.Administration.ApplicationSettings;

public class SetApplicationSettings
{
    public sealed record SetApplicationSettingsRequest(string WebsiteUrl);

    public sealed record SetApplicationSettingsResult(string WebsiteUrl);

    public sealed class
        SetApplicationSettingsEndpoint : Endpoint<SetApplicationSettingsRequest>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Post("administration/application-settings");
        }

        public override async Task HandleAsync(SetApplicationSettingsRequest req, CancellationToken ct)
        {
            var applicationSettings = await DbContext.ApplicationSettings.SingleOrDefaultAsync(ct);

            if (applicationSettings is null)
            {
                DbContext.ApplicationSettings.Add(
                    Domain.ApplicationSettings.ApplicationSettings.Initialize(req.WebsiteUrl));
            }
            else
            {
                applicationSettings.Update(req.WebsiteUrl);
                DbContext.ApplicationSettings.Update(applicationSettings);
            }

            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new { }, cancellation: ct);
        }
    }
}