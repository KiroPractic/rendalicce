using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Chats;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Chats;

public sealed class GetChats
{
    public sealed record GetChatsResponse(IEnumerable<Chat> Chats);

    public sealed class GetChatsEndpoint : EndpointWithoutRequest<GetChatsResponse>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Get("chats");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var chats = await DbContext.Chats
                .Include(c => c.Participants)
                .Where(c => c.Participants.Any(u => u.Id == HttpContext.GetAuthenticatedUser().Id)).ToListAsync(ct);
            
            await SendAsync(new(chats), cancellation: ct);
        }
    }
}