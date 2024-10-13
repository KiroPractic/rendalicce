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
    public string? PhoneNumber { get; private set; }
    public string? ProfilePhotoBase64 { get; private set; }
    private string PasswordHash { get; set; } = null!;
    private int CreditsBalance { get; set; }
    
    public string GetFullName() => $"{FirstName} {LastName}";
    
    public static User Initialize(string firstName, string lastName, string email, string? description, string? phoneNumber, string password)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Description = description,
            PhoneNumber = phoneNumber,
            PasswordHash = HashPassword(password),
            CreditsBalance = 100
        };
    }
    
    public void Update(string firstName, string lastName, string email, string? description, string? phoneNumber, string? profilePhotoBase64)
    {
        if(ProfilePhotoBase64 is null && !string.IsNullOrEmpty(profilePhotoBase64))
            AddCredits(50);
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Description = description;
        PhoneNumber = phoneNumber;
        ProfilePhotoBase64 = profilePhotoBase64 ?? ProfilePhotoBase64;
    }

    public int GetCreditsValance() => CreditsBalance;
    
    public void AddCredits(int credits)
    {
        CreditsBalance += credits;
    }
    
    public void DeductCredits(int credits)
    {
        CreditsBalance -= credits;
    }
    
    public void SetPassword(string password) => PasswordHash = HashPassword(password);
    public bool IsCorrectPassword(string password) => HashPassword(password) == PasswordHash;
    private static string HashPassword(string password) => BitConverter.ToString(SHA512.HashData(Encoding.UTF8.GetBytes(password))).Replace("-", string.Empty);
}