using FluentValidation;

namespace Rendalicce.Domain.Users;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        // Rule for minimum 8 characters
        RuleFor(password => password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        // Rule for at least one digit
        RuleFor(password => password)
            .Matches(@"\d").WithMessage("Password must contain at least one digit.");

        // Rule for at least one uppercase letter
        RuleFor(password => password)
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.");

        // Rule for at least one lowercase letter
        RuleFor(password => password)
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.");

        // Rule for at least one special character
        RuleFor(password => password)
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
    }
}