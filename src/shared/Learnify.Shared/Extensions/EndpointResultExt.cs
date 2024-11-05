namespace Learnify.Shared.Extensions;

public static class EndpointResultExt
{
    public static async Task<IResult> ToGenericResultAsync(this Task<ServiceResult> taskServiceResult)
    {
        var serviceResult = await taskServiceResult;

        return serviceResult.Status switch
        {
            StatusCodes.Status204NoContent => Results.NoContent(),
            StatusCodes.Status404NotFound => Results.NotFound(serviceResult.Fail),
            _ => Results.Problem(serviceResult.Fail)
        };
    }

    public static async Task<IResult> ToGenericResultAsync<T>(this Task<ServiceResult<T>> taskServiceResult) where T : class
    {
        var serviceResult = await taskServiceResult;

        return serviceResult.Status switch
        {
            StatusCodes.Status200OK => Results.Ok(serviceResult.Data),
            StatusCodes.Status201Created => Results.Created(serviceResult.UrlAsCreated, serviceResult.Data),
            StatusCodes.Status404NotFound => Results.NotFound(serviceResult.Fail),
            _ => Results.Problem(serviceResult.Fail)
        };
    }
}
