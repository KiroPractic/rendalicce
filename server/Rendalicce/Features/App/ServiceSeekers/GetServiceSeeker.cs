using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Persistency;
using ServiceSeeker = Rendalicce.Domain.ServiceSeekers.ServiceSeeker;

namespace Rendalicce.Features.App.ServiceSeekers;

public sealed class GetServiceSeeker
{
    public sealed record GetServiceSeekerRequest(Guid Id);
    public sealed record GetServiceSeekerResult(ServiceSeeker ServiceSeeker);

    public sealed class GetServiceSeekerEndpoint : Endpoint<GetServiceSeekerRequest, GetServiceSeekerResult>
    {
        private readonly DatabaseContext _dbContext;

        public GetServiceSeekerEndpoint(DatabaseContext databaseContext, EmailSendingService emailSendingService) => _dbContext = databaseContext;

        public override void Configure()
        {
            AllowAnonymous();
            Get("service-seekers/{id}");
        }

        public override async Task HandleAsync(GetServiceSeekerRequest req, CancellationToken ct)
        {
            var serviceSeeker = await _dbContext.ServiceSeekers.FirstOrDefaultAsync(ss => ss.Id == req.Id, ct);
            if(serviceSeeker is null)
                ThrowError("Entitet ne postoji.");
            
            await SendAsync(new GetServiceSeekerResult(serviceSeeker), cancellation: ct);
        }
    }
}