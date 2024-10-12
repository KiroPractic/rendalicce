using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Features.Shared;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Infrastructure.Extensions;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class UpdateServiceProviderReview
{
    public sealed record UpdateServiceProviderReviewRequest(Guid Id, int Rating, string Content, IFormFile? ContentPhoto);


    public sealed class UpdateServiceProviderReviewEndpoint : Endpoint<UpdateServiceProviderReviewRequest, CreateOrUpdateEntityResult>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            AllowFileUploads();
            Put("service-providers/{id}/reviews");
        }

        public override async Task HandleAsync(UpdateServiceProviderReviewRequest req, CancellationToken ct)
        {
            var review = await DbContext.Reviews.FirstOrDefaultAsync(r => r.Id == req.Id && r.Reviewer.Id == HttpContext.GetAuthenticatedUser().Id, ct);
            if(review is null)
                ThrowError("Entitet ne postoji.");
            
            review.Update(req.Rating, req.Content, await req.ContentPhoto.ToBase64(ct));
            DbContext.Reviews.Update(review);
            await DbContext.SaveChangesAsync(ct);
            
            await SendAsync(new CreateOrUpdateEntityResult(review.Id), cancellation: ct);
        }
    }
}