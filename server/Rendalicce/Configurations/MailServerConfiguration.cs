using System.ComponentModel.DataAnnotations;

namespace Rendalicce.Configurations;

public class MailServerConfiguration
{
    [Required] public string Host { get; init; } = string.Empty;
    [Required] public int Port { get; init; }
    [Required] public bool EnableSsl { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    [Required] [EmailAddress] public string FromEmailAddress { get; init; } = string.Empty;
    [Required] public string FromDisplayName { get; init; } = string.Empty;
}