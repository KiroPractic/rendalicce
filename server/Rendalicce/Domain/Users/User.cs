using System.Security.Cryptography;
using System.Text;

namespace Rendalicce.Domain.Users;

public sealed class User : Entity
{
    private User() { }
    
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? ProfilePhotoBase64 { get; private set; }
    private string PasswordHash { get; set; } = null!;

    
    public static User Initialize(string firstName, string lastName, string email, string? description, string password)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Description = description,
            PasswordHash = HashPassword(password)
        };
    }

    public void Update(string firstName, string lastName, string email, string? description, string? profilePhotoBase64)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Description = description;
        ProfilePhotoBase64 = profilePhotoBase64;
    }
    
    public void SetPassword(string password) => PasswordHash = HashPassword(password);
    public bool IsCorrectPassword(string password) => HashPassword(password) == PasswordHash;
    private static string HashPassword(string password) => BitConverter.ToString(SHA512.HashData(Encoding.UTF8.GetBytes(password))).Replace("-", string.Empty);
}