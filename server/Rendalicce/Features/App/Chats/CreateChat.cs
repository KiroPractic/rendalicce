using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Chats;
    using Rendalicce.Infrastructure.Authentication;
    using Rendalicce.Persistency;
    
    namespace Rendalicce.Features.App.Chats;
    
    public sealed class GetChatWithUser
    {
        public sealed record GetChatWithUserRequest(Guid UserId);
    
        public sealed record GetChatWithUserResponse(Chat Chat);
    
        public sealed class GetChatWithUserEndpoint : Endpoint<GetChatWithUserRequest, GetChatWithUserResponse>
        {
            public required DatabaseContext DbContext { get; init; }
    
            public override void Configure()
            {
                Get("chats/users/{userId}");
            }
    
            public override async Task HandleAsync(GetChatWithUserRequest request, CancellationToken ct)
            {
                var chat = DbContext.Chats
                    .FirstOrDefault(c => c.Participants.Any(u => u.Id == request.UserId)
                                         && c.Participants.Any(u => u.Id == HttpContext.GetAuthenticatedUser().Id)
                                         && c.Participants.Count == 2);
    
                if (chat is null)
                {
                    var participant = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, ct);
                    if(participant is null)
                        ThrowError("Entitet ne postoji.");

                    chat = Chat.Initialize([HttpContext.GetAuthenticatedUser(), participant]);
                    DbContext.Chats.Add(chat);
                    await DbContext.SaveChangesAsync(ct);
                }
                else
                {
                    chat.SeenBy(HttpContext.GetAuthenticatedUser());
                    DbContext.Chats.Update(chat);
                    await DbContext.SaveChangesAsync(ct);
                }

                await SendAsync(new(chat), cancellation: ct);
            }
        }
    }