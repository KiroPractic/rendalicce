using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.Chats;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Persistency;

namespace Rendalicce.Features.App.Chats;

public class ChatHub(DatabaseContext dbContext) : Hub
{
    // When a user joins a chat with a specific chatId
    public async Task SubscribeToChatUpdates(Guid chatId)
    {
        var authenticatedUser = Context.GetHttpContext()?.GetAuthenticatedUser();
        var chat = authenticatedUser is null ? null : await dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId 
                                                                   && c.Participants.Any(u => u.Id == authenticatedUser.Id));
        if (authenticatedUser is null || chat is null)
        {
            await Clients.Caller.SendAsync("Error", "You are not authorized to join this chat.");
            return;
        }
        
        var groupName = $"{chatId}";
        // Add the current connection to a group associated with the chatId
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    // When a user leaves the chat
    public async Task UnsubscribeFromChatUpdates(Guid chatId)
    {
        var groupName = $"{chatId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    // Send a message to a specific chat group (chatId)
    public async Task SendMessageToChat(Guid chatId, string messageContent)
    {
        var authenticatedUser = Context.GetHttpContext()?.GetAuthenticatedUser();
        var chat = authenticatedUser is null ? null : await dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId 
            && c.Participants.Any(u => u.Id == authenticatedUser.Id));
        if (authenticatedUser is null || chat is null)
        {
            await Clients.Caller.SendAsync("Error", "You are not authorized to join this chat.");
            return;
        }

        var message = ChatMessage.Initialize(messageContent, authenticatedUser);
        chat.AddMessage(message);
        dbContext.Chats.Update(chat);
        await dbContext.SaveChangesAsync();
        
        var groupName = $"{chatId}";
        await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
    }
}