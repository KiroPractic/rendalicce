using FluentValidation;

namespace Rendalicce.Domain.Users;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        // Rule for minimum 8 characters
        RuleFor(password => password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Lozinka mora imati minimalno 8 znakova.");

        // Rule for at least one digit
        RuleFor(password => password)
            .Matches(@"\d").WithMessage("Lozinka mora sadržavati najmanje jednu znamenku.");

        // Rule for at least one uppercase letter
        RuleFor(password => password)
            .Matches(@"[A-Z]").WithMessage("Lozinka mora sadržavati veliko slovo.");

        // Rule for at least one lowercase letter
        RuleFor(password => password)
            .Matches(@"[a-z]").WithMessage("Lozinka mora sadržavati malo slovo.");

        // Rule for at least one special character
        RuleFor(password => password)
            .Matches(@"[\W_]").WithMessage("Lozinka mora sadržavati poseban znak.");
    }
}