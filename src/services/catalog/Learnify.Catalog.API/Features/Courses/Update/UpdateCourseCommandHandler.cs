namespace Learnify.Catalog.API.Features.Courses.Update;

public static class UpdateCourseCommandEndpoint
{
    public static RouteGroupBuilder UpdateCourseGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPut("/", async (UpdateCourseCommand command, IMediator mediator) =>
        {
            return await mediator.Send(command).ToGenericResultAsync();
        })
       .WithName("UpdateCourse")
       .Produces<ServiceResult>(StatusCodes.Status204NoContent)
       .AddEndpointFilter<ValidationFilter<UpdateCourseCommand>>();

        return routeGroupBuilder;
    }
}

public sealed class UpdateCourseCommandHandler(AppDbContext context)
    : IRequestHandler<UpdateCourseCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await context.Courses.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);
        if (course is null)
        {
            return ServiceResult.ErrorAsNotFound();
        }

        course.Name = request.Name;
        course.Description = request.Description;
        course.Price = request.Price;
        course.ImageUrl = request.ImageUrl;
        course.CategoryId = request.CategoryId;

        context.Courses.Update(course);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}
