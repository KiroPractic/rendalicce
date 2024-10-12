using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Persistency;
using ServiceProvider = Rendalicce.Domain.ServiceProviders.ServiceProvider;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class GetUsersServiceProviders
{
    public sealed record GetUsersServiceProvidersRequest(Guid UserId);
    public sealed record GetUsersServiceProvidersResult(IEnumerable<ServiceProvider> ServiceProviders);

    public sealed class GetUsersServiceProvidersEndpoint : Endpoint<GetUsersServiceProvidersRequest, GetUsersServiceProvidersResult>
    {
        private readonly DatabaseContext _dbContext;

        public GetUsersServiceProvidersEndpoint(DatabaseContext databaseContext, EmailSendingService emailSendingService) => _dbContext = databaseContext;

        public override void Configure()
        {
            Get("service-providers/users/{userId}");
        }

        public override async Task HandleAsync(GetUsersServiceProvidersRequest req, CancellationToken ct)
        {
            await SendAsync(new GetUsersServiceProvidersResult(await _dbContext.ServiceProviders.Where(sp => sp.Owner.Id == req.UserId).ToListAsync(ct)), cancellation: ct);
        }
    }
}