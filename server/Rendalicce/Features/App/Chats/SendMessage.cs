using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
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

        public required IHubContext<ChatHub> HubContext { get; init; }

        public override void Configure()
        {
            Post("chats/{id}/messages");
        }

        public override async Task HandleAsync(SendMessageRequest req, CancellationToken ct)
        {
            var chat = await DbContext.Chats
                .Include(c => c.Participants)
                .Include(c => c.Messages)
                .ThenInclude(m => m.SeenByParticipants)
                .FirstOrDefaultAsync(
                    c => c.Id == req.Id && c.Participants.Any(u => u.Id == HttpContext.GetAuthenticatedUser().Id), ct);

            if (chat is null)
                ThrowError("Entitet ne postoji.");

            var message = ChatMessage.Initialize(req.Content, HttpContext.GetAuthenticatedUser());
            chat.AddMessage(message);
            DbContext.ChatMessages.Add(message);
            DbContext.Chats.Update(chat);
            await DbContext.SaveChangesAsync(ct);
            
            await HubContext.Clients.Group($"{chat.Id}").SendAsync("NewMessage", message, cancellationToken: ct);
            
            await SendAsync(new(chat), cancellation: ct);
        }
    }
}