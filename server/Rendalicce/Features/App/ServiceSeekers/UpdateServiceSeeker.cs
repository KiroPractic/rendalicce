using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Features.Shared;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceSeekers;

public sealed class UpdateServiceSeeker
{
    public sealed record UpdateServiceSeekerRequest(Guid Id, string Name, string Description, string Category, string Tags, string Email, string? PhoneNumber, string? CompanyName, string Geolocation);

    public sealed class UpdateServiceSeekerEndpoint : Endpoint<UpdateServiceSeekerRequest, CreateOrUpdateEntityResult>
    {
        public DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            AllowFileUploads();
            Put("service-seekers/{id}");
        }

        public override async Task HandleAsync(UpdateServiceSeekerRequest req, CancellationToken ct)
        {
            var serviceSeeker = await DbContext.ServiceSeekers.FirstOrDefaultAsync(sp =>
                sp.Id == req.Id && sp.Owner.Id == HttpContext.GetAuthenticatedUser().Id, ct);

            if(serviceSeeker is null)
                ThrowError("Entitet ne postoji.");
            
            serviceSeeker.Update(req.Name, req.Description, req.Category, req.Geolocation, req.Email, req.PhoneNumber, req.CompanyName, req.Tags);
            DbContext.ServiceSeekers.Update(serviceSeeker);
            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new CreateOrUpdateEntityResult(serviceSeeker.Id), cancellation: ct);
        }
    }
}