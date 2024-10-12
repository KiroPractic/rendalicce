using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Persistency;
using ServiceProvider = Rendalicce.Domain.ServiceProviders.ServiceProvider;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class GetServiceProviders
{
    public sealed record GetServiceProvidersResult(IEnumerable<ServiceProvider> ServiceProviders);

    public sealed class GetServiceProvidersEndpoint : EndpointWithoutRequest<GetServiceProvidersResult>
    {
        private readonly DatabaseContext _dbContext;

        public GetServiceProvidersEndpoint(DatabaseContext databaseContext, EmailSendingService emailSendingService) => _dbContext = databaseContext;

        public override void Configure()
        {
            AllowAnonymous();
            Get("service-providers");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await SendAsync(new GetServiceProvidersResult(await _dbContext.ServiceProviders
                .Include(sp => sp.Owner)
                .ToListAsync(ct)), cancellation: ct);
        }
    }
}