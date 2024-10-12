using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Users;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Authentication;

public sealed class Register
{
    public sealed record RegisterRequest(string FirstName, string LastName, string Email, string? Description, string? PhoneNumber, string Password);
    public sealed record RegisterResult(string Token);

    public sealed class RegisterEndpoint(DatabaseContext databaseContext, JwtProvider jwtProvider)
        : Endpoint<RegisterRequest, RegisterResult>
    {
        public override void Configure()
        {
            AllowAnonymous();
            Post("auth/register");
        }

        public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
        {
            var existingUser = await databaseContext.Users.FirstOrDefaultAsync(u => u.Email == req.Email, ct);
            if (existingUser is not null)
                ThrowError("Email is already in use.");
            
            var user = Domain.Users.User.Initialize(req.FirstName, req.LastName, req.Email, req.Description, req.PhoneNumber, req.Password);
            
            databaseContext.Users.Add(user);
            await databaseContext.SaveChangesAsync(ct);

            await SendAsync(new RegisterResult(jwtProvider.GenerateJwtToken(user)), cancellation: ct);
        }
    }

    public sealed class RegisterRequestValidator : Validator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(r => r.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ime je obavezno.")
                .MaximumLength(63).WithMessage("Ime ne smije biti dulje od 63 znako.");
            
            RuleFor(r => r.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Prezime je obavezno.")
                .MaximumLength(63).WithMessage("Prezime ne smije biti dulje od 63 znako.");

            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("E-mail je obavezan.")
                .EmailAddress().WithMessage("E-mail nije valjan.");
            
            RuleFor(r => r.Password).SetValidator(new PasswordValidator());
        }
    }
}