namespace Rendalicce.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IServiceCollection AddSingletonConfiguration<TConfiguration>(this WebApplicationBuilder builder) where TConfiguration : class
    {
        var configurationSection = builder.Configuration.GetSection(typeof(TConfiguration).Name).Get<TConfiguration>();
        if(configurationSection is null)
            throw new InvalidOperationException($"Configuration for {typeof(TConfiguration).Name} is missing or invalid.");
        
        return builder.Services.AddSingleton(configurationSection);
    }
}