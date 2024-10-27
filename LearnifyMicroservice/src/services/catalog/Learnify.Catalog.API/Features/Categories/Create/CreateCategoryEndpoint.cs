namespace Learnify.Catalog.API.Features.Categories.Create;

public static class CreateCategoryEndpoint
{
    public static RouteGroupBuilder CreateCategoryGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/", async (CreateCategoryCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            return new ObjectResult(result)
            {
                StatusCode = result.Status.GetHashCode()
            };
        });

        return routeGroupBuilder;
    }
}
