using FastEndpoints;
using Rendalicce.Features.Shared;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;
using ServiceProvider = Rendalicce.Domain.ServiceProviders.ServiceProvider;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class CreateServiceProvider
{
    public sealed record CreateServiceProviderRequest(string Name, string Description, string Category, string Tags, string Email, string? PhoneNumber, string? CompanyName, decimal? Price, string PaymentType, string Geolocation);

    public sealed class CreateServiceProviderEndpoint : Endpoint<CreateServiceProviderRequest, CreateOrUpdateEntityResult>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Post("service-providers");
        }

        public override async Task HandleAsync(CreateServiceProviderRequest req, CancellationToken ct)
        {
            var serviceProvider = ServiceProvider.Initialize(req.Name, req.Description, req.Category, req.Tags, req.Geolocation, req.Email, req.PhoneNumber, req.CompanyName,  req.Price, req.PaymentType, HttpContext.GetAuthenticatedUser());

            DbContext.ServiceProviders.Add(serviceProvider);
            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new CreateOrUpdateEntityResult(serviceProvider.Id), cancellation: ct);
        }
    }
}