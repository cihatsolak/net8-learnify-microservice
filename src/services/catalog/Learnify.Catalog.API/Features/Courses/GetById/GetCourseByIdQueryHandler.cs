namespace Learnify.Catalog.API.Features.Courses.GetById;

public static class GetCourseByIdEndpoint
{
    public static RouteGroupBuilder GetCourseByIdGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            return await mediator.Send(new GetCourseByIdQuery(id)).ToGenericResultAsync();
        })
        .WithName("GetCourseById")
        .Produces<CourseResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return routeGroupBuilder;
    }
}

public sealed class GetCourseByIdQueryHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetCourseByIdQuery, ServiceResult<CourseResponse>>
{
    public async Task<ServiceResult<CourseResponse>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await context.Courses.FindAsync([request.Id, cancellationToken], cancellationToken);
        if (course is null)
        {
            return ServiceResult<CourseResponse>
                 .Error("Course not found.", $"The course with Id ({request.Id}) was not found.", StatusCodes.Status404NotFound);
        }

        course.Category = await context.Categories.FindAsync([course.CategoryId, cancellationToken], cancellationToken);
        
        return ServiceResult<CourseResponse>.SuccessAsOk(mapper.Map<CourseResponse>(course));
    }
}