using FluentEmail.Core;
using Rendalicce.Configurations;
using Rendalicce.Domain.Chats;
using Rendalicce.Infrastructure.Emails.Templates;

namespace Rendalicce.Infrastructure.Emails;

public class EmailSendingService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IWebHostEnvironment _environment;
    private readonly ApplicationErrorNotificationConfiguration _applicationErrorNotificationConfiguration;

    public EmailSendingService(IFluentEmail fluentEmail,
        ApplicationErrorNotificationConfiguration applicationErrorNotificationConfiguration,
        IWebHostEnvironment environment)
    {
        _fluentEmail = fluentEmail;
        _applicationErrorNotificationConfiguration = applicationErrorNotificationConfiguration;
        _environment = environment;
    }

    public Task SendPasswordResetRequested(string email, string firstName, string resetLink)
        =>
            _fluentEmail
                .To(email)
                .Subject("Password Reset")
                .UsingTemplateFromFile(
                    GetTemplateFilePath<PasswordResetRequestedTemplateModel>(),
                    new PasswordResetRequestedTemplateModel(firstName, resetLink)
                )
                .SendAsync();

    public Task SendNewChatMessageReceived(string recipientEmail, string recipientFirstName, string chatLink, string senderFullName, string messageContent)
        =>
            _fluentEmail
                .To(recipientEmail)
                .Subject("Obavijest o novoj poruci")
                .UsingTemplateFromFile(
                    GetTemplateFilePath<NewChatMessageReceivedTemplateModel>(),
                    new NewChatMessageReceivedTemplateModel(recipientFirstName, chatLink, senderFullName, messageContent)
                )
                .SendAsync();
    
    public Task SendHangfireFailureNotification(string jobId, string jobName,
        IReadOnlyDictionary<string, string> jobParametersSnapshot, Exception exception)
    {
        var email = _fluentEmail.Subject($"{_environment.ApplicationName} {_environment.EnvironmentName} - Background Task Failure");
        foreach (var recipient in _applicationErrorNotificationConfiguration.EmailRecipientList)
            email.To(recipient);

        return email
            .UsingTemplateFromFile(
                GetTemplateFilePath<HangfireFailureTemplateModel>(),
                new HangfireFailureTemplateModel(jobId, jobName, jobParametersSnapshot, exception.Message,
                    exception.InnerException?.Message)
            )
            .SendAsync();
    }

    public Task SendExceptionOccurrenceNotification(Exception exception, HttpContext httpContext)
    {
        var email = _fluentEmail.Subject($"{_environment.ApplicationName} {_environment.EnvironmentName} - Exception Occurrence");
        foreach (var recipient in _applicationErrorNotificationConfiguration.EmailRecipientList)
            email.To(recipient);

        return email
            .UsingTemplateFromFile(
                GetTemplateFilePath<ExceptionOccurrenceTemplateModel>(),
                new ExceptionOccurrenceTemplateModel(exception, httpContext)
            )
            .SendAsync();
    }


    private static string GetTemplateFilePath<TModelClass>()
    {
        return $"Infrastructure/Emails/Templates/{typeof(TModelClass).Name.Replace("Model", "")}.cshtml";
    }
}