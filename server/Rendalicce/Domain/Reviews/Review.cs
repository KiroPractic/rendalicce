using Rendalicce.Domain.Users;

namespace Rendalicce.Domain.Reviews;

public class Review : Entity
{
    private Review() { }
    
    public int Rating { get; private set; }
    public string Content { get; private set; } = null!;
    public string? ContentPhotoBase64 { get; private set; }
    public Guid RevieweeId { get; init; }
    public User Reviewer { get; init; } = null!;
    public bool Banned { get; set; }
    
    public static Review Initialize(int rating, string content, string? contentPhotoBase64, Entity reviewee, User reviewer)
    {
        return new Review
        {
            Rating = rating,
            Content = content,
            ContentPhotoBase64 = contentPhotoBase64,
            RevieweeId = reviewee.Id,
            Reviewer = reviewer
        };
    }

    public void Update(int rating, string content, string? contentPhotoBase64)
    {
        Rating = rating;
        Content = content;
        ContentPhotoBase64 = contentPhotoBase64;
    }
}