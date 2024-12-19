using System.Net;

namespace Hyzen.SDK.Exception;

public partial class HException
{
    public static HException Unauthorized(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.InvalidCredentials, HttpStatusCode.Unauthorized, innerException);
    }

    public static HException NotFound(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.NotFound, HttpStatusCode.NotFound, innerException);
    }

    public static HException BadRequest(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.InvalidOperation, HttpStatusCode.BadRequest, innerException);
    }

    public static HException InternalError(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.InternalError, HttpStatusCode.InternalServerError, innerException);
    }

    public static HException Forbidden(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.PermissionRequired, HttpStatusCode.Forbidden, innerException);
    }

    public static HException Timeout(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.Timeout, HttpStatusCode.RequestTimeout, innerException);
    }

    public static HException TooManyRequests(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.TooManyRequests, HttpStatusCode.TooManyRequests, innerException);
    }

    public static HException MissingParams(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.MissingParams, HttpStatusCode.BadRequest, innerException);
    }

    public static HException NotImplemented(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.NotImplemented, HttpStatusCode.NotImplemented, innerException);
    }

    public static HException BadGateway(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.BadGateway, HttpStatusCode.BadGateway, innerException);
    }

    public static HException InvalidParams(string message, System.Exception innerException = null)
    {
        return new HException(message, ExceptionType.InvalidParams, HttpStatusCode.BadRequest, innerException);
    }
}