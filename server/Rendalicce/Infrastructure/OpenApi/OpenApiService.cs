using System.Text.Json.Serialization;
using OpenAI;
using OpenAI.Chat;
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

    public static async Task<IEnumerable<string>> GetTags(string serviceName, string serviceDescription, string serviceCategory)
    {
        using var api = new OpenAIClient(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        var messages = new List<Message>
        {
            new(Role.System, "Ti si servis za izradu tagova na hrvatskom jeziku, iz opisa i naziva pružanja usluge. Odgovor vrati u JSON formati."),
            new(Role.System, "Vrati najviše 5 i najmanje 3 taga. U obzir uzmi samo riječi koje su ključne za samu uslugu."),
            new(Role.User, $"Generiraj mi listu tagova za uslugu naslova \"{serviceName}\", opisa \"{serviceDescription}\" i kategorije \"{serviceCategory}\".")
        };

        var chatRequest = new ChatRequest(messages, model: Model.GPT4o);
        var (tagsResponse, _) = await api.ChatEndpoint.GetCompletionAsync<TagsResponse>(chatRequest);
        
        return tagsResponse.Tags;
    }
    
    private static async Task<byte[]> DownloadImageAsync(string imageUrl)
    {
        using var response = await HttpClient.GetAsync(imageUrl);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsByteArrayAsync();
    }
    
    public class TagsResponse
    {
        [JsonInclude]
        [JsonPropertyName("tags")]
        public IEnumerable<string> Tags { get; private set; } = null!;
    }
}