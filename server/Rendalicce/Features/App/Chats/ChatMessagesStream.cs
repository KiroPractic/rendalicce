using System.Runtime.CompilerServices;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Chats;

public sealed class ChatMessagesStream
{
    public sealed record ChatMessagesStreamRequest(Guid Id);
    
    public class ChatMessagesStreamEndpoint : Endpoint<ChatMessagesStreamRequest>
    {
        public required DatabaseContext DbContext { get; init; }

        public override void Configure()
        {
            Get("chats/{id}/messages");
        }

        public override async Task HandleAsync(ChatMessagesStreamRequest req, CancellationToken ct)
        {
            var chat = await DbContext.Chats
                .FirstOrDefaultAsync(c => c.Id == req.Id
                                          && c.Participants.Any(u => u.Id == HttpContext.GetAuthenticatedUser().Id),
                    ct);
            
            if (chat is null)
                ThrowError("Entitet ne postoji.");

            //simply provide any IAsyncEnumerable<T> as argument
            await SendEventStreamAsync("my-event", GetDataStream(ct), ct);
        }

        private async IAsyncEnumerable<object> GetDataStream([EnumeratorCancellation] CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(1000);
                yield return new { guid = Guid.NewGuid() };
            }
        }
    }
}