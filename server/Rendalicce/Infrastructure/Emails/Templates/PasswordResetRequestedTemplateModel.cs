namespace Rendalicce.Infrastructure.Emails.Templates;

public sealed class PasswordResetRequestedTemplateModel(string firstName, string resetLink)
{
    public string FirstName { get; } = firstName;
    public string ResetLink { get; } = resetLink;
}