namespace Rendalicce.Infrastructure.Emails.Templates;

public class ExceptionOccurrenceTemplateModel(Exception exception, HttpContext httpContext)
{
    public string ExceptionMessage { get; } = exception.Message;
    public string? ExceptionStackTrace { get; } = exception.StackTrace;
    public List<InnerExceptionInformation> InnerExceptions { get; } = ExtractInnerExceptions(exception);
    public string? RequestUrl { get; } = httpContext.Request.Path.Value;
    public string HttpMethod { get; } = httpContext.Request.Method;
    public string? QueryString { get; } = httpContext.Request.QueryString.Value;
    public Dictionary<string, string> RequestHeaders { get; } =
        httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

    private static List<InnerExceptionInformation> ExtractInnerExceptions(Exception exception)
    {
        if (exception.InnerException is null)
            return [];

        var result = new List<InnerExceptionInformation> { new(exception.InnerException) };
        result.AddRange(ExtractInnerExceptions(exception.InnerException));
        return result;
    }

    public class InnerExceptionInformation(Exception exception)
    {
        public string Message { get; } = exception.Message;
        public string? StackTrace { get; } = exception.StackTrace;
    }
}