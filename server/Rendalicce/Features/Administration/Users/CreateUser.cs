using FastEndpoints;
using FluentValidation;
using Rendalicce.Domain.Users;
using Rendalicce.Features.Shared;
using Rendalicce.Persistency;

namespace Rendalicce.Features.Administration.Users;

public sealed class CreateUser
{
    public sealed record CreateUserRequest(string FirstName, string LastName, string Email, string Password);

    public sealed class CreateUserEndpoint : Endpoint<CreateUserRequest, CreateOrUpdateEntityResult>
    {
        private readonly DatabaseContext _dbContext;

        public CreateUserEndpoint(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public override void Configure()
        {
            Post("administration/users");
        }

        public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
        {
            var user = Domain.Users.User.Initialize(req.FirstName, req.LastName, req.Email, req.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(ct);

            await SendAsync(new CreateOrUpdateEntityResult(user.Id), cancellation: ct);
        }
    }

    public sealed class CreateUserRequestValidator : Validator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(r => r.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name cannot be empty.")
                .MaximumLength(63).WithMessage("First name must have length less than 63 characters.");
            
            RuleFor(r => r.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name cannot be empty.")
                .MaximumLength(63).WithMessage("Last name must have length less than 63 characters.");

            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Invalid email address.");
            
            RuleFor(r => r.Password).SetValidator(new PasswordValidator());
        }
    }
}