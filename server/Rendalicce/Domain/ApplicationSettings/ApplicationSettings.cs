namespace Rendalicce.Domain.ApplicationSettings;

public class ApplicationSettings : Entity
{
    public string WebsiteRootUrl { get; private set; } = null!;

    public static ApplicationSettings Initialize(string websiteRootUrl) => new() { WebsiteRootUrl = websiteRootUrl };

    public void Update(string websiteRootUrl)
    {
        WebsiteRootUrl = websiteRootUrl;
    }
}