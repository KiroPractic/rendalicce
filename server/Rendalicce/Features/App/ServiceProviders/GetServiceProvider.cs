using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Reviews;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Persistency;
using ServiceProvider = Rendalicce.Domain.ServiceProviders.ServiceProvider;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class GetServiceProvider
{
    public sealed record GetServiceProviderRequest(Guid Id);
    public sealed record GetServiceProviderResult(ServiceProvider ServiceProvider, IEnumerable<Review> Reviews);

    public sealed class GetServiceProviderEndpoint : Endpoint<GetServiceProviderRequest, GetServiceProviderResult>
    {
        private readonly DatabaseContext _dbContext;

        public GetServiceProviderEndpoint(DatabaseContext databaseContext, EmailSendingService emailSendingService) => _dbContext = databaseContext;

        public override void Configure()
        {
            AllowAnonymous();
            Get("service-providers/{id}");
        }

        public override async Task HandleAsync(GetServiceProviderRequest req, CancellationToken ct)
        {
            var serviceProvider = await _dbContext.ServiceProviders
                .Include(sp => sp.Owner)
                .FirstOrDefaultAsync(sp => sp.Id == req.Id, ct);
            if(serviceProvider is null)
                ThrowError("Entitet ne postoji.");
            
            var reviews = await _dbContext.Reviews.Where(r => r.RevieweeId == serviceProvider.Id && r.Banned == false).ToListAsync(ct);
            
            await SendAsync(new GetServiceProviderResult(serviceProvider, reviews), cancellation: ct);
        }
    }
}