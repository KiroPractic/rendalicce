namespace Rendalicce.Domain.ServiceTransactions;

public sealed class ServiceTransaction : Entity
{
    private ServiceTransaction() { }

    public List<ServiceTransactionParticipant> Participants { get; set; } = new();

    public static ServiceTransaction Initialize(IEnumerable<ServiceTransactionParticipant> participants)
    {
        return new ServiceTransaction
        {
            Participants = participants.ToList()
        };
    }
}