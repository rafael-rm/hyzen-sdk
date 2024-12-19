using System.Net;

namespace Hyzen.SDK.Exception;

public partial class HException : System.Exception
{
    public ExceptionType Type { get; }
    public HttpStatusCode StatusCode { get; }

    private static readonly Dictionary<ExceptionType, HttpStatusCode> ExceptionToStatusCode = new()
    {
        { ExceptionType.InvalidCredentials, HttpStatusCode.Unauthorized },
        { ExceptionType.NotFound, HttpStatusCode.NotFound },
        { ExceptionType.InvalidOperation, HttpStatusCode.BadRequest },
        { ExceptionType.PermissionRequired, HttpStatusCode.Forbidden },
        { ExceptionType.Timeout, HttpStatusCode.RequestTimeout },
        { ExceptionType.TooManyRequests, HttpStatusCode.TooManyRequests },
        { ExceptionType.MissingParams, HttpStatusCode.BadRequest },
        { ExceptionType.NotImplemented, HttpStatusCode.NotImplemented },
        { ExceptionType.InternalError, HttpStatusCode.InternalServerError },
        { ExceptionType.BadGateway, HttpStatusCode.BadGateway },
        { ExceptionType.InvalidParams, HttpStatusCode.BadRequest },
    };

    public HException(string message, ExceptionType type, HttpStatusCode? statusCode = null, System.Exception innerException = null) : base(message, innerException)
    {
        Type = type;
        StatusCode = statusCode ?? ExceptionToStatusCode.GetValueOrDefault(type, HttpStatusCode.InternalServerError);
    }
    
    public static HException FromException(System.Exception exception)
    {
        if (exception is HException hException)
            return hException;

        return new HException("An internal error occurred", ExceptionType.InternalError, HttpStatusCode.InternalServerError, exception);
    }
    
    public object ToErrorObject()
    {
        return new
        {
            error = Type.ToString(),
            errorDescription = Message,
        };
    }
}
