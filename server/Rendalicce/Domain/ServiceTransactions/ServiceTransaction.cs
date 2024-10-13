using Rendalicce.Domain.Users;

namespace Rendalicce.Domain.ServiceTransactions;

public sealed class ServiceTransaction : Entity
{
    private ServiceTransaction() { }

    public bool Completed { get; private set; }
    public DateTimeOffset CompletedOn { get; private set; }
    public List<ServiceTransactionParticipant> Participants { get; set; } = new();

    public static ServiceTransaction Initialize(IEnumerable<ServiceTransactionParticipant> participants)
    {
        return new ServiceTransaction
        {
            Participants = participants.ToList()
        };
    }

    public void Approve(User user)
    {
        var participant = Participants.SingleOrDefault(p => p.Id == user.Id);
        participant!.Approve();

        if (Participants.All(p => p.Approved))
            Complete();
    }
    
    public void Complete()
    {
        Completed = true;
        CompletedOn = DateTimeOffset.UtcNow;
        Participants.ForEach(p => p.Complete());
    }
}