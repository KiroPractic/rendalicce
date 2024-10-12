using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Persistency;
using ServiceSeeker = Rendalicce.Domain.ServiceSeekers.ServiceSeeker;

namespace Rendalicce.Features.App.ServiceSeekers;

public sealed class GetServiceSeekers
{
    public sealed record GetServiceSeekersResult(IEnumerable<ServiceSeeker> ServiceSeekers);

    public sealed class GetServiceSeekersEndpoint : EndpointWithoutRequest<GetServiceSeekersResult>
    {
        private readonly DatabaseContext _dbContext;

        public GetServiceSeekersEndpoint(DatabaseContext databaseContext, EmailSendingService emailSendingService) => _dbContext = databaseContext;

        public override void Configure()
        {
            AllowAnonymous();
            Get("service-seekers");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await SendAsync(new GetServiceSeekersResult(await _dbContext.ServiceSeekers.ToListAsync(ct)), cancellation: ct);
        }
    }
}