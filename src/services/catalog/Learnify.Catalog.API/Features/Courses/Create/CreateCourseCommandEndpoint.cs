namespace Learnify.Catalog.API.Features.Courses.Create;

public static class CreateCourseCommandEndpoint
{
    public static RouteGroupBuilder CreateCourseGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/", async (CreateCourseCommand command, IMediator mediator)
            => await mediator.Send(command).ToGenericResultAsync());

        routeGroupBuilder.AddEndpointFilter<ValidationFilter<CreateCourseCommandValidator>>();

        return routeGroupBuilder;
    }
}
