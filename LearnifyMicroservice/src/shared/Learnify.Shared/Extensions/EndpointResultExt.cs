namespace Learnify.Shared.Extensions;

public static class EndpointResultExt
{
    public static IResult ToGenericResult(this ServiceResult serviceResult)
    {
        return serviceResult.Status switch
        {
            StatusCodes.Status204NoContent => Results.NoContent(),
            StatusCodes.Status404NotFound => Results.NotFound(serviceResult.Fail),
            _ => Results.Problem(serviceResult.Fail)
        };
    }

    public static IResult ToGenericResult<T>(this ServiceResult<T> serviceResult) where T : class
    {
        return serviceResult.Status switch
        {
            StatusCodes.Status200OK => Results.Ok(serviceResult),
            StatusCodes.Status201Created => Results.Created(serviceResult.UrlAsCreated, serviceResult),
            StatusCodes.Status404NotFound => Results.NotFound(serviceResult.Fail),
            _ => Results.Problem(serviceResult.Fail)
        };
    }
}
