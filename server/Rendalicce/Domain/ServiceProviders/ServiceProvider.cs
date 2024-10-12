using Rendalicce.Domain.Reviews;
using Rendalicce.Domain.Users;

namespace Rendalicce.Domain.ServiceProviders;

public sealed class ServiceProvider : Entity
{
    private ServiceProvider() { }
    
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public string Tags { get; set; } = null!;
    public string Geolocation { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? CompanyName { get; set; }
    public decimal? Price { get; set; }
    public string PaymentType { get; set; } = null!;
    public string? HeaderPhotoBase64 { get; private set; }
    public User Owner { get; init; } = null!;
    public IEnumerable<Review> Reviews { get; private set; } = new List<Review>();
    
    public static ServiceProvider Initialize(string name, string description, string category, string tags, string geolocation, string email, string? phoneNumber, string? companyName, decimal? price, string paymentType, string? headerPhotoBase64, User owner)
    {
        return new ServiceProvider
        {
            Name = name,
            Description = description,
            Category = category,
            Tags = tags,
            Geolocation = geolocation,
            Email = email,
            PhoneNumber = phoneNumber,
            CompanyName = companyName,
            Price = price,
            PaymentType = paymentType,
            HeaderPhotoBase64 = headerPhotoBase64,
            Owner = owner
        };
    }

    public void Update(string name, string description, string category, string geolocation, string email, string? phoneNumber, string? companyName, decimal? price, string paymentType, string tags, string? headerPhotoBase64)
    {
        Name = name;
        Description = description;
        Category = category;
        Geolocation = geolocation;
        Email = email;
        PhoneNumber = phoneNumber;
        CompanyName = companyName;
        Price = price;
        PaymentType = paymentType;
        Tags = tags;
        HeaderPhotoBase64 = headerPhotoBase64;
    }
}