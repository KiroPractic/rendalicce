using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Persistency;
using ServiceSeeker = Rendalicce.Domain.ServiceSeekers.ServiceSeeker;

namespace Rendalicce.Features.App.ServiceSeekers;

public sealed class GetUsersServiceSeekers
{
    public sealed record GetUsersServiceSeekersResult(IEnumerable<ServiceSeeker> ServiceSeekers);

    public sealed class GetUsersServiceSeekersEndpoint : EndpointWithoutRequest<GetUsersServiceSeekersResult>
    {
        private readonly DatabaseContext _dbContext;

        public GetUsersServiceSeekersEndpoint(DatabaseContext databaseContext, EmailSendingService emailSendingService) => _dbContext = databaseContext;

        public override void Configure()
        {
            Get("service-seekers/user");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var authenticatedUser = HttpContext.GetAuthenticatedUser()!;
            await SendAsync(new GetUsersServiceSeekersResult(await _dbContext.ServiceSeekers.Where(sp => sp.Owner.Id == authenticatedUser.Id).ToListAsync(ct)), cancellation: ct);
        }
    }
}