namespace Rendalicce.Infrastructure.Extensions;

public static class FormFileExtensions
{
    public static async Task<string> ToBase64(this IFormFile file, CancellationToken ct)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);
        var fileBytes = ms.ToArray();
        return Convert.ToBase64String(fileBytes);
    }
}