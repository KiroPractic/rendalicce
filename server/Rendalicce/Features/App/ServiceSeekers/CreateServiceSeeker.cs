using FastEndpoints;
using Rendalicce.Features.Shared;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceSeekers;

public sealed class CreateServiceSeeker
{
    public sealed record CreateServiceSeekerRequest(string Name, string Description, string Category, string Tags, string Email, string? PhoneNumber, string? CompanyName, string Geolocation);

    public sealed class CreateServiceSeekerEndpoint : Endpoint<CreateServiceSeekerRequest, CreateOrUpdateEntityResult>
    {
        public DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Post("service-seekers");
        }

        public override async Task HandleAsync(CreateServiceSeekerRequest req, CancellationToken ct)
        {
            var serviceSeeker = Domain.ServiceSeekers.ServiceSeeker.Initialize(req.Name, req.Description, req.Category, req.Tags, req.Geolocation, req.Email, req.PhoneNumber, req.CompanyName, HttpContext.GetAuthenticatedUser()!);

            DbContext.ServiceSeekers.Add(serviceSeeker);
            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new CreateOrUpdateEntityResult(serviceSeeker.Id), cancellation: ct);
        }
    }
}