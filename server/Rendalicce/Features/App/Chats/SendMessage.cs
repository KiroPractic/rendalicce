using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Chats;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Chats;

public sealed class SendMessage
{
    public sealed record SendMessageRequest(Guid Id, string Content);

    public sealed record SendMessageResponse(Chat Chat);

    public sealed class SendMessageEndpoint : Endpoint<SendMessageRequest, SendMessageResponse>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Post("chats/{id}/message");
        }

        public override async Task HandleAsync(SendMessageRequest req, CancellationToken ct)
        {
            var chat = await DbContext.Chats
                .FirstOrDefaultAsync(c => c.Id == req.Id && c.Participants.Any(u => u.Id == HttpContext.GetAuthenticatedUser().Id), ct);

            if (chat is null)
                ThrowError("Entitet ne postoji.");

            chat.AddMessage(ChatMessage.Initialize(req.Content, HttpContext.GetAuthenticatedUser()));
            DbContext.Chats.Update(chat);
            await DbContext.SaveChangesAsync(ct);
            
            await SendAsync(new(chat), cancellation: ct);
        }
    }
}