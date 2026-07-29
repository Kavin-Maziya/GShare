using GearShare.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GearShare.Api.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Domain exceptions are expected failures — log at Warning.
        // Unhandled exceptions are bugs — log at Error with the full exception
        // attached as structured data, NOT string-interpolated into the message.
        if (exception is GearNotFoundException or GearNotAvailableException)
            logger.LogWarning("Domain exception {ExceptionType}: {Message}",
                exception.GetType().Name, exception.Message);
        else
            logger.LogError(exception, "An unhandled exception occurred: {Message}",
                exception.Message);

        var statusCode = exception switch
        {
            UnauthorizedActionException  => StatusCodes.Status403Forbidden,
            GearNotFoundException     => StatusCodes.Status404NotFound,
            GearNotAvailableException => StatusCodes.Status422UnprocessableEntity,
            _                         => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status   = statusCode,
            Title    = GetTitle(statusCode),
            // For 500s detail is intentionally null — internal messages must
            // never reach the client. For known domain exceptions it is safe.
            Detail   = statusCode == StatusCodes.Status500InternalServerError
                           ? null
                           : exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status403Forbidden           => "Access denied.",
        StatusCodes.Status404NotFound               => "Gear item not found.",
        StatusCodes.Status422UnprocessableEntity    => "Gear item is not available.",
        _                                           => "An unexpected error occurred."
    };
}