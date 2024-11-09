namespace Learnify.Catalog.API.Features.Courses.Create;

public static class CreateCourseCommandEndpoint
{
    public static RouteGroupBuilder CreateCourseGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/", async (CreateCourseCommand command, IMediator mediator)
            => await mediator.Send(command).ToGenericResultAsync())
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        routeGroupBuilder.WithName("CreateCourse");
        routeGroupBuilder.AddEndpointFilter<ValidationFilter<CreateCourseCommandValidator>>();
        

        return routeGroupBuilder;
    }
}
