using FastEndpoints;
using Rendalicce.Infrastructure.Authentication;

namespace Rendalicce.Features.App.Account;

public sealed class GetAccountInformation
{
    public sealed record GetAccountInformationResult(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string? Description,
        string? PhoneNumber,
        string? ProfilePhotoBase64,
        int CreditsBalance);

    public sealed class GetAccountInformationEndpoint : EndpointWithoutRequest<GetAccountInformationResult>
    {
        public override void Configure()
        {
            Get("account");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var user = HttpContext.GetAuthenticatedUserOrNull()!;
            await SendAsync(new GetAccountInformationResult(
                    Id: user.Id,
                    FirstName: user.FirstName,
                    LastName: user.LastName,
                    Email: user.Email,
                    Description: user.Description,
                    PhoneNumber: user.PhoneNumber,
                    user.ProfilePhotoBase64,
                    user.GetCreditsValance()), cancellation:
                ct);
        }
    }
}