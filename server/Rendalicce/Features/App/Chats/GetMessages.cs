using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Chats;

public sealed class GetMessages
{
    public sealed record GetChatMessagesRequest(Guid Id);

    public sealed record GetChatMessagesResponse(IEnumerable<ChatMessageInformation> ChatMessages);

    public sealed record ChatMessageInformation(Guid Id, string Content, Guid SenderId, DateTimeOffset CreatedOn);

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
                .Select(cm => new ChatMessageInformation(cm.Id, cm.Content, cm.Sender.Id, cm.CreatedOn))
                .ToListAsync(ct);

            await SendAsync(new(messages), cancellation: ct);
        }
    }
}