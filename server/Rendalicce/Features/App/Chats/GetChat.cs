using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Chats;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Chats;

public sealed class GetChat
{
    public sealed record GetChatRequest(Guid Id);

    public sealed record GetChatResponse(Chat Chat);

    public sealed class GetChatEndpoint : Endpoint<GetChatRequest, GetChatResponse>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Get("chats/{id}");
        }

        public override async Task HandleAsync(GetChatRequest req, CancellationToken ct)
        {
            var chat = await DbContext.Chats
                .Include(c => c.Participants)
                .Include(c => c.Messages)
                .ThenInclude(m => m.SeenByParticipants)
                .Include(c => c.Messages)
                .ThenInclude(cm => cm.ServiceTransaction)
                .ThenInclude(st => st!.Participants)
                .ThenInclude(o => o.User)

                .FirstOrDefaultAsync(c => c.Id == req.Id
                                          && c.Participants.Any(u => u.Id == HttpContext.GetAuthenticatedUser().Id),
                    ct);

            if (chat is null)
                ThrowError("Entitet ne postoji.");

            chat.SeenBy(HttpContext.GetAuthenticatedUser());
            DbContext.Chats.Update(chat);
            await DbContext.SaveChangesAsync(ct);

            await SendAsync(new(chat), cancellation: ct);
        }
    }
}