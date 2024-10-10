using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Authentication;

public sealed class Authenticate
{
    public sealed record AuthenticateRequest(string Email, string Password, bool RememberMe);

    public sealed record AuthenticateResult(string Token);

    public sealed class AuthenticateEndpoint : Endpoint<AuthenticateRequest, AuthenticateResult>
    {
        private readonly DatabaseContext _dbContext;
        private readonly JwtProvider _jwtProvider;
        
        public AuthenticateEndpoint(DatabaseContext databaseContext, JwtProvider jwtProvider)
        {
            _dbContext = databaseContext;
            _jwtProvider = jwtProvider;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Post("auth/login");
        }

        public override async Task HandleAsync(AuthenticateRequest req, CancellationToken ct)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == req.Email, ct);
            if(user is null || !user.IsCorrectPassword(req.Password))
                ThrowError("Invalid credentials.");
            
            await SendAsync(new AuthenticateResult(_jwtProvider.GenerateJwtToken(user, req.RememberMe)), cancellation: ct);
        }
    }

    public sealed class AuthenticateRequestValidator : Validator<AuthenticateRequest>
    {
        public AuthenticateRequestValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email cannot be empty.");
            
            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password cannot be empty.");
        }
    }
}