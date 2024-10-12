using Rendalicce.Domain.Users;

namespace Rendalicce.Domain.ServiceSeekers;

public sealed class ServiceSeeker : Entity
{
    private ServiceSeeker() { }
    
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public string Tags { get; set; } = null!;
    public string Geolocation { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? CompanyName { get; set; }
    public User Owner { get; init; } = null!;
    
    
    public static ServiceSeeker Initialize(string name, string description, string category, string tags, string geolocation, string email, string phoneNumber, string companyName, User owner)
    {
        return new ServiceSeeker
        {
            Name = name,
            Description = description,
            Category = category,
            Tags = tags,
            Geolocation = geolocation,
            Email = email,
            PhoneNumber = phoneNumber,
            CompanyName = companyName,
            Owner = owner
        };
    }

    public void Update(string name, string description, string category, string geolocation, string email, string? phoneNumber, string? companyName, string tags)
    {
        Name = name;
        Description = description;
        Category = category;
        Geolocation = geolocation;
        Email = email;
        PhoneNumber = phoneNumber;
        CompanyName = companyName;
        Tags = tags;
    }
}