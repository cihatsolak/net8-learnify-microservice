namespace Learnify.Shared.Filters;

public class ValidationFilter<TRequest> : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<TRequest>>();
        if (validator is null)
        {
            return await next(context);
        }

        var requestModel = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (requestModel is null)
        {
            return await next(context);
        }

        var validationResult = await validator.ValidateAsync(requestModel, context.HttpContext.RequestAborted);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        return await next(context);
    }
}
