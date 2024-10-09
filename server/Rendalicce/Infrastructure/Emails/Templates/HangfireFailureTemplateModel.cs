namespace Rendalicce.Infrastructure.Emails.Templates;

public sealed class HangfireFailureTemplateModel(
    string jobId,
    string jobName,
    IReadOnlyDictionary<string, string> jobParametersSnapshot,
    string exceptionMessage,
    string? innerExceptionMessage = null)
{
    public string JobId { get; } = jobId;
    public string JobName { get; } = jobName;
    public IReadOnlyDictionary<string, string> JobParametersSnapshot { get; } = jobParametersSnapshot;
    public string ExceptionMessage { get; } = exceptionMessage;
    public string? InnerExceptionMessage { get; } = innerExceptionMessage;
}