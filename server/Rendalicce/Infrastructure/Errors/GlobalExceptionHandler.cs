using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Diagnostics;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Infrastructure.Logging;

namespace Rendalicce.Infrastructure.Errors;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly Type[] _excludedExceptions =
    [
        typeof(OperationCanceledException),
        typeof(TaskCanceledException),
        typeof(ConnectionResetException)
    ];
    
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (_excludedExceptions.Contains(exception.GetType()))
            return false;

        _logger.LogCritical(exception, "{LogEventType}", LogEventType.UnhandledExceptionOccurred);
        
        await httpContext.Resolve<EmailSendingService>().SendExceptionOccurrenceNotification(exception, httpContext);

        await httpContext.Response
            .WriteAsJsonAsync(
                new ValidationFailure("InternalServerError", "UnexpectedErrorOccurred"),
                cancellationToken
            );

        return true;
    }
}