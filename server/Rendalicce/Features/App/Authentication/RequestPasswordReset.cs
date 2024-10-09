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
        private readonly DatabaseContext _dbContext;
        private readonly EmailSendingService _emailSendingService;
        
        public RequestPasswordResetEndpoint(DatabaseContext databaseContext, AuthenticationProvider authenticationProvider, EmailSendingService emailSendingService)
        {
            _dbContext = databaseContext;
            _emailSendingService = emailSendingService;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Post("auth/request-password-reset");
        }

        public override async Task HandleAsync(RequestPasswordResetRequest req, CancellationToken ct)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == req.Email, ct);
            if (user is not null)
                _ = _emailSendingService.SendPasswordResetRequested(user.Email, user.FirstName, "TODO-FK");
            
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