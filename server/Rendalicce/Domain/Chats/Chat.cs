using Rendalicce.Domain.Users;

namespace Rendalicce.Domain.Chats;

public class Chat : Entity
{
    private Chat() { }
    
    public DateTimeOffset LastActivity { get; private set; }
    public List<ChatMessage> Messages { get; init; } = new();
    public List<User> Participants { get; init; } = new();
    
    
    public static Chat Initialize(IEnumerable<User> participants)
    {
        return new Chat
        {
            LastActivity = DateTimeOffset.UtcNow,
            Participants = participants.ToList()
        };
    }

    public void AddMessage(ChatMessage message)
    {
        Messages.Add(message);
        LastActivity = DateTimeOffset.UtcNow;
        SeenBy(message.Sender);
    }

    public void SeenBy(User user)
    {
        Messages.ForEach(m => m.SeenBy(user));
    }
}