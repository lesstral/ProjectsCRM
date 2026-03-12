using System.Net;
using ApplicationCore.Enums;

namespace Web.Utils;

public static class ErrorMapper
{
    public static HttpStatusCode ToStatusCode(this ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => HttpStatusCode.BadRequest, // 400
            ErrorType.NotFound => HttpStatusCode.NotFound, // 404
            ErrorType.Unauthorized => HttpStatusCode.Unauthorized, // 401
            ErrorType.Forbidden => HttpStatusCode.Forbidden, // 403
            ErrorType.Conflict => HttpStatusCode.Conflict, // 409
            ErrorType.InternalServerError => HttpStatusCode.InternalServerError, // 500
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
