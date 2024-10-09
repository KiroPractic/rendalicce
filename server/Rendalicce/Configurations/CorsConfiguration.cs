using System.ComponentModel.DataAnnotations;

namespace Rendalicce.Configurations;

public sealed class CorsConfiguration
{
    [Required]
    public string[] Origins { get; init; } = [];
}