using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Reviews;
using Rendalicce.Features.Shared;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Infrastructure.Extensions;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class CreateServiceProviderReview
{
    public sealed record CreateServiceProviderReviewRequest(Guid Id, int Rating, string Content, IFormFile? ContentPhoto);


    public sealed class CreateServiceProviderReviewEndpoint : Endpoint<CreateServiceProviderReviewRequest, CreateOrUpdateEntityResult>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            AllowFileUploads();
            Post("service-providers/{id}/reviews");
        }

        public override async Task HandleAsync(CreateServiceProviderReviewRequest req, CancellationToken ct)
        {
            var serviceProvider = await DbContext.ServiceProviders.FirstOrDefaultAsync(sp => sp.Id == req.Id, ct);
            if(serviceProvider is null)
                ThrowError("Entitet ne postoji.");
            
            var review = Review.Initialize(req.Rating, req.Content, await req.ContentPhoto.ToBase64(ct), serviceProvider, HttpContext.GetAuthenticatedUser());
            DbContext.Reviews.Add(review);
            await DbContext.SaveChangesAsync(ct);
            
            await SendAsync(new CreateOrUpdateEntityResult(review.Id), cancellation: ct);
        }
    }
}