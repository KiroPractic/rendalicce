namespace Rendalicce.Infrastructure.Interaction;

public class Response
{
    protected Response()
    {
    }

    public Response(Dictionary<string, string[]> errors) => Errors = errors;

    public Dictionary<string, string[]>? Errors { get; }
    public bool HasErrors => Errors is not null;
}

public sealed class Response<T> : Response
{
    public Response(T result) => Result = result;

    public T Result { get; }
}