using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Chats;

public sealed class GetMessages
{
    public sealed record GetChatMessagesRequest(Guid Id);

    public sealed record GetChatMessagesResponse(IEnumerable<ChatMessageInformation> ChatMessages);

    public sealed record ServiceTransactionInformation(
        Guid Id,
        bool Completed,
        DateTimeOffset CompletedOn,
        IEnumerable<ServiceTransactionParticipantInformation> Participants);

    public sealed record ServiceTransactionParticipantInformation(
        Guid Id,
        bool Approved,
        int? Credits,
        Guid? ProvidedServiceId,
        Guid UserId);

    public sealed record ChatMessageInformation(
        Guid Id,
        string Content,
        Guid SenderId,
        DateTimeOffset CreatedOn,
        ServiceTransactionInformation? ServiceTransaction);

    public sealed class GetChatMessagesEndpoint : Endpoint<GetChatMessagesRequest, GetChatMessagesResponse>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Get("chats/{id}/messages");
        }

        public override async Task HandleAsync(GetChatMessagesRequest req, CancellationToken ct)
        {
            // TODO-FK: Add chat id validation.
            // TODO-FK: Add seen by.

            var messages = await DbContext.ChatMessages
                .Where(cm => cm.ChatId == req.Id)
                .Include(cm => cm.ServiceTransaction)
                .ThenInclude(st => st!.Participants)
                .ThenInclude(o => o.User)
                .Select(cm => new ChatMessageInformation(cm.Id, cm.Content, cm.Sender.Id, cm.CreatedOn,
                        cm.ServiceTransaction == null
                            ? null
                            : new ServiceTransactionInformation(
                                cm.ServiceTransaction.Id,
                                cm.ServiceTransaction.Completed,
                                cm.ServiceTransaction.CompletedOn,
                                cm.ServiceTransaction.Participants.Select(p =>
                                    new ServiceTransactionParticipantInformation(
                                        p.Id,
                                        p.Approved,
                                        p.Credits,
                                        p.ProvidedService == null ?  null : p.ProvidedService.Id,
                                        p.User.Id
                                    )
                                )
                            )))
                    .ToListAsync(ct);

            await SendAsync(new(messages), cancellation: ct);
        }
    }
}