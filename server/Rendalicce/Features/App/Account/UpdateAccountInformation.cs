using FastEndpoints;
using FluentValidation;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Account;

public sealed class UpdateAccountInformation
{
    public sealed record UpdateAccountInformationRequest(
        string FirstName,
        string LastName,
        IFormFile? ProfilePhoto,
        string Email);

    public sealed record UpdateAccountInformationResult(string Token);

    public sealed class UpdateAccountEndpoint : Endpoint<UpdateAccountInformationRequest, UpdateAccountInformationResult>
    {
        public DatabaseContext DbContext { get; init; }
        public JwtProvider JwtProvider { get; init; }

        public override void Configure()
        {
            AllowFileUploads();
            Post("account");
        }

        public override async Task HandleAsync(UpdateAccountInformationRequest req, CancellationToken ct)
        {
            var user = HttpContext.GetAuthenticatedUserOrNull();
            var profilePhotoBase64 = req.ProfilePhoto is null ? null : await GetBase64(req.ProfilePhoto, ct);
            user!.Update(req.FirstName, req.LastName, req.Email, profilePhotoBase64);
            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new UpdateAccountInformationResult(Token: JwtProvider.GenerateJwtToken(user)), cancellation: ct);
        }

        private static async Task<string> GetBase64(IFormFile file, CancellationToken ct)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, ct);
            var fileBytes = ms.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
    }

    public sealed class UpdateAccountRequestValidator : Validator<UpdateAccountInformationRequest>
    {
        // TODO-FK: Reuse the existing error messages.
        public UpdateAccountRequestValidator()
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
        }
    }
}