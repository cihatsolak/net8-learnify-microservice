namespace Learnify.Shared.ExceptionHandlers;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = HttpStatusCode.InternalServerError.GetHashCode();
        
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Title = "An error occurred while processing your request",
            Type = exception.GetType().Name,
            Status = HttpStatusCode.InternalServerError.GetHashCode()
        }, cancellationToken);

        return true;
    }
}
