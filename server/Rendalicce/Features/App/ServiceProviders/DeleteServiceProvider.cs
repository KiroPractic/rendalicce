using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Features.Shared;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class DeleteServiceProvider
{
    public sealed record DeleteServiceProviderRequest(Guid Id);

    public sealed class DeleteServiceProviderEndpoint : Endpoint<DeleteServiceProviderRequest, CreateOrUpdateEntityResult>
    {
        public DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Delete("service-providers/{id}");
        }

        public override async Task HandleAsync(DeleteServiceProviderRequest req, CancellationToken ct)
        {
            var serviceProvider = await DbContext.ServiceProviders.FirstOrDefaultAsync(sp =>
                sp.Id == req.Id && sp.Owner.Id == HttpContext.GetAuthenticatedUser().Id, ct);

            if(serviceProvider is null)
                ThrowError("Entitet ne postoji.");
            
            DbContext.ServiceProviders.Remove(serviceProvider);
            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new CreateOrUpdateEntityResult(serviceProvider.Id), cancellation: ct);
        }
    }
}