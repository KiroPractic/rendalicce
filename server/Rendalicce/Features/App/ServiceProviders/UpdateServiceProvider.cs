using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Features.Shared;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Infrastructure.Extensions;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class UpdateServiceProvider
{
    public sealed record UpdateServiceProviderRequest(Guid Id, string Name, string Description, string Category, string Tags, string Email, string? PhoneNumber, string? CompanyName, decimal Price, string PaymentType, string Geolocation, IFormFile? HeaderPhoto);

    public sealed class UpdateServiceProviderEndpoint : Endpoint<UpdateServiceProviderRequest, CreateOrUpdateEntityResult>
    {
        public DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            AllowFileUploads();
            Put("service-providers/{id}");
        }

        public override async Task HandleAsync(UpdateServiceProviderRequest req, CancellationToken ct)
        {
            var serviceProvider = await DbContext.ServiceProviders.FirstOrDefaultAsync(sp =>
                sp.Id == req.Id && sp.Owner.Id == HttpContext.GetAuthenticatedUser().Id, ct);

            if(serviceProvider is null)
                ThrowError("Entitet ne postoji.");

            var headerPhotoBase64 = req.HeaderPhoto is null ? null : await req.HeaderPhoto.ToBase64(ct);
            serviceProvider.Update(req.Name, req.Description, req.Category, req.Geolocation, req.Email, req.PhoneNumber, req.CompanyName, req.Price, req.PaymentType, req.Tags, headerPhotoBase64);
            DbContext.ServiceProviders.Update(serviceProvider);
            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new CreateOrUpdateEntityResult(serviceProvider.Id), cancellation: ct);
        }
    }
}