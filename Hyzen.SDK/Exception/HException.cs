using System.Net;
using System.Security.Authentication;

namespace Hyzen.SDK.Exception;

public class HException : System.Exception
{
    public ExceptionType Type { get; }
    public HttpStatusCode StatusCode { get; set; }

    public HException(string message) : base(message)
    {
        Type = ExceptionType.InternalError;
        StatusCode = GetDefaultStatusCodeForException(Type);
    }
    
    public HException(string message, ExceptionType type) : base(message)
    {
        Type = type;
        StatusCode = GetDefaultStatusCodeForException(type);
    }

    public HException(string message, ExceptionType type, System.Exception innerException) : base(message, innerException)
    {
        Type = type;
        StatusCode = GetDefaultStatusCodeForException(type);
    }
    
    public HException(string message, ExceptionType type, HttpStatusCode statusCode) : base(message)
    {
        Type = type;
        StatusCode = statusCode;
    }
    
    public HException(string message, ExceptionType type, System.Exception innerException, HttpStatusCode statusCode) : base(message, innerException)
    {
        Type = type;
        StatusCode = statusCode;
    }
    
    public HException(string message, HttpStatusCode statusCode, System.Exception innerException, ExceptionType type) : base(message, innerException)
    {
        Type = type;
        StatusCode = statusCode;
    }
    
    public HException(string message, HttpStatusCode statusCode) : base(message)
    {
        Type = GetDefaultExceptionTypeForStatusCode(statusCode);
        StatusCode = statusCode;
    }
    
    public HException(string message, HttpStatusCode statusCode, System.Exception innerException) : base(message, innerException)
    {
        Type = GetDefaultExceptionTypeForStatusCode(statusCode);
        StatusCode = statusCode;
    }
    
    public HException(HttpStatusCode statusCode) : base(GetDefaultExceptionTypeForStatusCode(statusCode).ToString())
    {
        Type = GetDefaultExceptionTypeForStatusCode(statusCode);
        StatusCode = statusCode;
    }
    
    public static HException FromException(System.Exception exception)
    {
        return exception switch
        {
            HException e => e,
            ArgumentException e => new HException(e.Message, ExceptionType.InvalidParams, e),
            InvalidOperationException e => new HException(e.Message, ExceptionType.InvalidOperation, e),
            TimeoutException e => new HException(e.Message, ExceptionType.Timeout, e),
            AuthenticationException e => new HException(e.Message, ExceptionType.InvalidCredentials, e),
            UnauthorizedAccessException e => new HException(e.Message, ExceptionType.PermissionRequired, e),
            FormatException e => new HException(e.Message, ExceptionType.InvalidParams, e),
            KeyNotFoundException e => new HException(e.Message, ExceptionType.NotFound, e),
            IndexOutOfRangeException e => new HException(e.Message, ExceptionType.NotFound, e),
            NotSupportedException e => new HException(e.Message, ExceptionType.NotImplemented, e),
            NotImplementedException e => new HException(e.Message, ExceptionType.NotImplemented, e),
            _ => new HException(exception.Message, ExceptionType.InternalError, exception)
        };
    }

    private static HttpStatusCode GetDefaultStatusCodeForException(ExceptionType type)
    {
        return type switch
        {
            ExceptionType.InvalidCredentials => HttpStatusCode.Unauthorized,
            ExceptionType.NotFound => HttpStatusCode.NotFound,
            ExceptionType.InvalidOperation => HttpStatusCode.BadRequest,
            ExceptionType.PermissionRequired => HttpStatusCode.Forbidden,
            ExceptionType.Timeout => HttpStatusCode.RequestTimeout,
            ExceptionType.TooManyRequests => HttpStatusCode.TooManyRequests,
            ExceptionType.MissingParams => HttpStatusCode.BadRequest,
            ExceptionType.NotImplemented => HttpStatusCode.NotImplemented,
            ExceptionType.InternalError => HttpStatusCode.InternalServerError,
            ExceptionType.BadGateway => HttpStatusCode.BadGateway,
            ExceptionType.InvalidParams => HttpStatusCode.BadRequest,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static ExceptionType GetDefaultExceptionTypeForStatusCode(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.Unauthorized => ExceptionType.InvalidCredentials,
            HttpStatusCode.NotFound => ExceptionType.NotFound,
            HttpStatusCode.BadRequest => ExceptionType.InvalidOperation,
            HttpStatusCode.Forbidden => ExceptionType.PermissionRequired,
            HttpStatusCode.RequestTimeout => ExceptionType.Timeout,
            HttpStatusCode.TooManyRequests => ExceptionType.TooManyRequests,
            HttpStatusCode.NotImplemented => ExceptionType.NotImplemented,
            HttpStatusCode.InternalServerError => ExceptionType.InternalError,
            HttpStatusCode.BadGateway => ExceptionType.BadGateway,
            HttpStatusCode.Conflict => ExceptionType.InvalidOperation,
            HttpStatusCode.Gone => ExceptionType.InvalidOperation,
            HttpStatusCode.LengthRequired => ExceptionType.MissingParams,
            HttpStatusCode.PreconditionFailed => ExceptionType.InvalidOperation,
            HttpStatusCode.RequestEntityTooLarge => ExceptionType.InvalidParams,
            HttpStatusCode.RequestUriTooLong => ExceptionType.InvalidParams,
            HttpStatusCode.UnsupportedMediaType => ExceptionType.InvalidParams,
            HttpStatusCode.RequestedRangeNotSatisfiable => ExceptionType.InvalidParams,
            HttpStatusCode.ExpectationFailed => ExceptionType.InvalidParams,
            HttpStatusCode.MisdirectedRequest => ExceptionType.InvalidParams,
            HttpStatusCode.UnprocessableEntity => ExceptionType.InvalidOperation,
            HttpStatusCode.Locked => ExceptionType.InvalidOperation,
            HttpStatusCode.FailedDependency => ExceptionType.InvalidOperation,
            HttpStatusCode.UpgradeRequired => ExceptionType.InvalidOperation,
            HttpStatusCode.PreconditionRequired => ExceptionType.InvalidOperation,
            HttpStatusCode.RequestHeaderFieldsTooLarge => ExceptionType.InvalidOperation,
            HttpStatusCode.UnavailableForLegalReasons => ExceptionType.InvalidOperation,
            HttpStatusCode.ServiceUnavailable => ExceptionType.InvalidOperation,
            HttpStatusCode.GatewayTimeout => ExceptionType.Timeout,
            HttpStatusCode.HttpVersionNotSupported => ExceptionType.InvalidOperation,
            HttpStatusCode.VariantAlsoNegotiates => ExceptionType.InvalidOperation,
            HttpStatusCode.InsufficientStorage => ExceptionType.InvalidOperation,
            HttpStatusCode.LoopDetected => ExceptionType.InvalidOperation,
            HttpStatusCode.NotExtended => ExceptionType.InvalidOperation,
            HttpStatusCode.NetworkAuthenticationRequired => ExceptionType.InvalidOperation,
            _ => ExceptionType.InternalError
        };
    }
    
    public object ToErrorObject()
    {
        var error = Type switch
        {
            ExceptionType.InvalidCredentials => nameof(ExceptionType.InvalidCredentials),
            ExceptionType.NotFound => nameof(ExceptionType.NotFound),
            ExceptionType.InvalidOperation => nameof(ExceptionType.InvalidOperation),
            ExceptionType.PermissionRequired => nameof(ExceptionType.PermissionRequired),
            ExceptionType.Timeout => nameof(ExceptionType.Timeout),
            ExceptionType.TooManyRequests => nameof(ExceptionType.TooManyRequests),
            ExceptionType.MissingParams => nameof(ExceptionType.MissingParams),
            ExceptionType.NotImplemented => nameof(ExceptionType.NotImplemented),
            ExceptionType.InternalError => nameof(ExceptionType.InternalError),
            ExceptionType.BadGateway => nameof(ExceptionType.BadGateway),
            ExceptionType.InvalidParams => nameof(ExceptionType.InvalidParams),
            _ => throw new ArgumentOutOfRangeException()
        };

        return new
        {
            error,
            errorDescription = Message
        };
    }
}