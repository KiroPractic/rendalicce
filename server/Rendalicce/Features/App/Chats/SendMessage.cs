using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Chats;
using Rendalicce.Domain.ServiceTransactions;
using Rendalicce.Features.Shared;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Chats;

public sealed class SendMessage
{
    public sealed record SendMessageRequest(Guid Id, string Content, ServiceTransactionInformation? ServiceTransaction);

    public sealed record ServiceTransactionInformation(IEnumerable<ServiceTransactionParticipantInformation> Participants);
    public sealed record ServiceTransactionParticipantInformation(int? Credits, Guid? ServiceProviderId, Guid? UserId);
    

    public sealed class SendMessageEndpoint : Endpoint<SendMessageRequest, CreateOrUpdateEntityResult>
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

            ServiceTransaction? serviceTransaction = null;
            if (req.ServiceTransaction is not null)
            {
                var participants = new List<ServiceTransactionParticipant>();
                foreach (var participantInformation in req.ServiceTransaction.Participants)
                {
                    var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == participantInformation.UserId, ct);
                    if(user is null)
                        ThrowError("Entitet ne postoji.");
                    
                    var serviceProvider = await DbContext.ServiceProviders.FirstOrDefaultAsync(sp => sp.Id == participantInformation.ServiceProviderId, ct);
                    if(participantInformation.ServiceProviderId is not null && serviceProvider is null)
                        ThrowError("Entitet ne postoji.");
                    
                    participants.Add(ServiceTransactionParticipant.Initialize(participantInformation.Credits, serviceProvider, user, user.Id == HttpContext.GetAuthenticatedUser().Id));
                }
                
                serviceTransaction = ServiceTransaction.Initialize(participants);
            }
            
            var message = ChatMessage.Initialize(req.Content, serviceTransaction, HttpContext.GetAuthenticatedUser());
            chat.AddMessage(message);
            DbContext.ChatMessages.Add(message);
            DbContext.Chats.Update(chat);
            await DbContext.SaveChangesAsync(ct);
            
            await HubContext.Clients.Group($"{chat.Id}").SendAsync("NewMessage", message, cancellationToken: ct);
            await HubContext.Clients.All.SendAsync("NewMessage", message, cancellationToken: ct);
            
            await SendAsync(new(chat.Id), cancellation: ct);
        }
    }
}