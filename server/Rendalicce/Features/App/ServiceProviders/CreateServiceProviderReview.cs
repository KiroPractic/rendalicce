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
    public sealed record CreateServiceProviderReviewRequest(
        Guid Id,
        int Rating,
        string Content,
        IFormFile? ContentPhoto);


    public sealed class
        CreateServiceProviderReviewEndpoint : Endpoint<CreateServiceProviderReviewRequest, CreateOrUpdateEntityResult>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            AllowFileUploads();
            Post("service-providers/{id}/reviews");
        }

        public override async Task HandleAsync(CreateServiceProviderReviewRequest req, CancellationToken ct)
        {
            var serviceProvider = await DbContext.ServiceProviders
                .Include(sp => sp.Owner)
                .FirstOrDefaultAsync(sp => sp.Id == req.Id, ct);
            if (serviceProvider is null)
                ThrowError("Entitet ne postoji.");

            var authenticatedUser = HttpContext.GetAuthenticatedUser();
            var revieweeUserOwner = serviceProvider.Owner;
            var review = Review.Initialize(req.Rating, req.Content, await req.ContentPhoto.ToBase64(ct),
                serviceProvider, authenticatedUser);
            DbContext.Reviews.Add(review);
            revieweeUserOwner.AddCredits(10);
            authenticatedUser.AddCredits(10);
            DbContext.Users.Update(revieweeUserOwner);
            DbContext.Users.Update(authenticatedUser);
            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new CreateOrUpdateEntityResult(review.Id), cancellation: ct);
        }
    }
}