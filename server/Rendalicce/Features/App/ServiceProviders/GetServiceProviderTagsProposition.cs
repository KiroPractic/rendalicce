using FastEndpoints;
using Rendalicce.Infrastructure.OpenApi;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class GetServiceProviderTagsProposition
{
    public sealed record GetServiceProviderTagsPropositionRequest(string Name, string Description, string Category);
    public sealed record GetServiceProviderTagsPropositionResult(IEnumerable<string> Tags);

    public sealed class GetServiceProviderTagsPropositionEndpoint : Endpoint<GetServiceProviderTagsPropositionRequest, GetServiceProviderTagsPropositionResult>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Post("service-providers/tags-proposition");
        }

        public override async Task HandleAsync(GetServiceProviderTagsPropositionRequest req, CancellationToken ct)
        {
            var tags= await OpenApiService.GetTags(req.Name, req.Description, req.Category);
            await SendAsync(new GetServiceProviderTagsPropositionResult(tags), cancellation: ct);
        }
    }
}