namespace Learnify.Shared.Extensions;

public static class ProblemDetailsExtensions
{
    public static void LogProblemDetails(this ILogger logger, ApiException apiException)
    {
        if (apiException is null)
        {
            logger.LogWarning("ApiException is null, nothing to log.");
            return;
        }

        if (string.IsNullOrWhiteSpace(apiException.Content))
        {
            logger.LogError("API Exception: {Message}", apiException.Message);
            return;
        }

        try
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(apiException.Content);
            if (problemDetails is null)
            {
                logger.LogError("API Exception could not be deserialized: {Message}", apiException.Message);
                return;
            }

            logger.LogError(
                "Problem Details - Title: {Title}, Detail: {Detail}, Status: {Status}",
                problemDetails.Title,
                problemDetails.Detail,
                problemDetails.Status);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to deserialize ProblemDetails: {Content}", apiException.Content);
        }
    }
}
