namespace Learnify.Catalog.API.Features.Courses.Delete;

public static class DeleteCourseEndpoint
{
    public static RouteGroupBuilder DeleteCourseGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            return await mediator.Send(new DeleteCourseCommand(id)).ToGenericResultAsync();
        })
       .WithName("DeleteCourse")
       .Produces<NoContentResult>(StatusCodes.Status204NoContent)
       .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
       .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return routeGroupBuilder;
    }
}

public sealed class DeleteCourseHandler(AppDbContext context) : IRequestHandler<DeleteCourseCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await context.Courses.FindAsync([request.Id, cancellationToken], cancellationToken);
        if (course is null)
        {
            return ServiceResult.ErrorAsNotFound();
        }

        context.Courses.Remove(course);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}
