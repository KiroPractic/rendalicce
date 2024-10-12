using FastEndpoints;
using Rendalicce.Infrastructure.OpenApi;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.ServiceProviders;

public sealed class GetServiceProviderHeaderPhotoProposition
{
    public sealed record GetServiceProviderHeaderPhotoPropositionRequest(string Name, string Description, string Category, string Tags);
    public sealed record GetServiceProviderHeaderPhotoPropositionResult(string HeaderPhotoProposition);

    public sealed class GetServiceProviderHeaderPhotoPropositionEndpoint : Endpoint<GetServiceProviderHeaderPhotoPropositionRequest, GetServiceProviderHeaderPhotoPropositionResult>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Post("service-providers/header-photo-proposition");
        }

        public override async Task HandleAsync(GetServiceProviderHeaderPhotoPropositionRequest req, CancellationToken ct)
        {
            var generationPrompt = $"Generiraj naslovnu sliku za uslugu naziva: {req.Name}, opisa: {req.Description}, kategorije {req.Category} i sa tagovima {req.Tags}.";
            var propositionImage= await OpenApiService.GenerateImage(generationPrompt);
            await SendAsync(new GetServiceProviderHeaderPhotoPropositionResult(propositionImage), cancellation: ct);
        }
    }
}