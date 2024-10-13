using Rendalicce.Domain.ServiceTransactions;
using Rendalicce.Domain.Users;

namespace Rendalicce.Domain.Chats;

public class ChatMessage : Entity
{
    private ChatMessage() { }

    public string Content { get; private set; } = null!;
    public ServiceTransaction? ServiceTransaction { get; set; }
    public User Sender { get; init; } = null!;
    public string OriginalContent { get; private set; } = null!;
    public bool Updated { get; private set; }
    public List<User> SeenByParticipants { get; private set; } = new();
    public Guid ChatId { get; private set; }
    
    public static ChatMessage Initialize(string content, ServiceTransaction? ServiceTransaction, User sender)
    {
        return new ChatMessage
        {
            OriginalContent = content,
            ServiceTransaction = ServiceTransaction,
            Content = content,
            Sender = sender
        };
    }

    public void Update(string content)
    {
        Content = content;
        Updated = true;
    }

    public void SeenBy(User user)
    {
        if(!SeenByParticipants.Contains(user))
            SeenByParticipants.Add(user);
    }
}