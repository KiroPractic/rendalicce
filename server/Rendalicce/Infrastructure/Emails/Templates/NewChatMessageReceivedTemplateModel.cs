namespace Rendalicce.Infrastructure.Emails.Templates;

public sealed class NewChatMessageReceivedTemplateModel(string recipientFirstName, string chatLink, string senderFullName, string messageContent)
{
    public string RecipientFirstName { get; } = recipientFirstName;
    public string ChatLink { get; } = chatLink;
    public string SenderFullName { get; } = senderFullName;
    public string MessageContent { get; } = messageContent;
}