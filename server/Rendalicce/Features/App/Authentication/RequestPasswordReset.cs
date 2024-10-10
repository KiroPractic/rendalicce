using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Authentication;

public sealed class RequestPasswordReset
{
    public sealed record RequestPasswordResetRequest(string Email);
    

    public sealed class RequestPasswordResetEndpoint : Endpoint<RequestPasswordResetRequest, object>
    {
        public required DatabaseContext DbContext { get; init; }
        public required EmailSendingService EmailSendingService { get; init; }
        public required JwtProvider JwtProvider { get; init; }
        

        public override void Configure()
        {
            AllowAnonymous();
            Post("auth/request-password-reset");
        }

        public override async Task HandleAsync(RequestPasswordResetRequest req, CancellationToken ct)
        {
            var user = await DbContext.Users.SingleOrDefaultAsync(u => u.Email == req.Email, ct);
            var settings = await DbContext.ApplicationSettings.FirstAsync(ct);
            if (user is not null)
                _ = EmailSendingService.SendPasswordResetRequested(user.Email, user.FirstName, $"{settings.WebsiteRootUrl}/auth/password-reset/{JwtProvider.GeneratePasswordResetToken(user.Id)}");
            
            await SendAsync(new {}, cancellation: ct);
        }
    }

    public sealed class RequestPasswordResetRequestValidator : Validator<RequestPasswordResetRequest>
    {
        public RequestPasswordResetRequestValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email cannot be empty.");
        }
    }
}