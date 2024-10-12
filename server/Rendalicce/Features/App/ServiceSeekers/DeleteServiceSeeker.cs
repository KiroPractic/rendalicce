using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Features.Shared;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceSeekers;

public sealed class DeleteServiceSeeker
{
    public sealed record DeleteServiceSeekerRequest(Guid Id);

    public sealed class DeleteServiceSeekerEndpoint : Endpoint<DeleteServiceSeekerRequest, CreateOrUpdateEntityResult>
    {
        public DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Delete("service-seekers/{id}");
        }

        public override async Task HandleAsync(DeleteServiceSeekerRequest req, CancellationToken ct)
        {
            var serviceSeeker = await DbContext.ServiceSeekers.FirstOrDefaultAsync(ss =>
                ss.Id == req.Id && ss.Owner.Id == HttpContext.GetAuthenticatedUser().Id, ct);

            if(serviceSeeker is null)
                ThrowError("Entitet ne postoji.");
            
            DbContext.ServiceSeekers.Remove(serviceSeeker);
            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new CreateOrUpdateEntityResult(serviceSeeker.Id), cancellation: ct);
        }
    }
}