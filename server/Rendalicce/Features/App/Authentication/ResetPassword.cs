using FastEndpoints;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Users;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Authentication;

public sealed class ResetPassword
{
    public sealed record ResetPasswordRequest(string Token, string Password);

    public sealed record ResetPasswordResponse(string Token);

    public sealed class ResetPasswordEndpoint : Endpoint<ResetPasswordRequest, ResetPasswordResponse>
    {
        public required DatabaseContext DbContext { get; init; }
        public required JwtProvider JwtProvider { get; init; }

        public override void Configure()
        {
            Post("auth/reset-password");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ResetPasswordRequest request, CancellationToken ct)
        {
            var userId = JwtProvider.GetUserIdFromPasswordResetToken(request.Token);
            if(userId is null)
                ThrowError(new ValidationFailure("Token", "Invalid token"));
            
            var user = await DbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId, ct);
            
            if(user is null)
                ThrowError(new ValidationFailure("Token", "Invalid token"));

            await SendAsync(new(Token: JwtProvider.GenerateJwtToken(user)), cancellation: ct);
        }
    }

    public sealed class ResetPasswordValidator : Validator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(r => r.Password).SetValidator(new PasswordValidator());
        }
    }
}