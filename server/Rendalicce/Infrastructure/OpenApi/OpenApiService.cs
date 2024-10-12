using OpenAI;
using OpenAI.Images;
using OpenAI.Models;

namespace Rendalicce.Infrastructure.OpenApi;

public static class OpenApiService
{
    private static readonly HttpClient HttpClient = new();

    public static async Task<string> GenerateImage(string generationPrompt)
    {
        using var api = new OpenAIClient(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        var request = new ImageGenerationRequest(generationPrompt, Model.DallE_3);
        var imageResults = await api.ImagesEndPoint.GenerateImageAsync(request);
        var imageUrl = imageResults[0]; // Retrieve the URL of the first image

        var imageBytes = await DownloadImageAsync(imageUrl);
        var base64String = Convert.ToBase64String(imageBytes);

        return base64String;
    }

    private static async Task<byte[]> DownloadImageAsync(string imageUrl)
    {
        using var response = await HttpClient.GetAsync(imageUrl);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsByteArrayAsync();
    }
}