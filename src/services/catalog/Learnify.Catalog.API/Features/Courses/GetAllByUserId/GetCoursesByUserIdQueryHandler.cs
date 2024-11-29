namespace Learnify.Catalog.API.Features.Courses.GetAllByUserId;

public sealed record GetCoursesByUserIdQuery(Guid Id) : IRequestResult<List<CourseResponse>>;

public sealed class GetCoursesByUserIdQueryHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetCoursesByUserIdQuery, ServiceResult<List<CourseResponse>>>
{
    public async Task<ServiceResult<List<CourseResponse>>> Handle(GetCoursesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await context.Courses.Where(course => course.UserId == request.Id).ToListAsync(cancellationToken);
        var categories = await context.Categories.ToListAsync(cancellationToken);

        courses.ForEach(course =>
        {
            course.Category = categories.First(category => category.Id == course.CategoryId);
        });

        var coursesAsResponse = mapper.Map<List<CourseResponse>>(courses);
        return ServiceResult<List<CourseResponse>>.SuccessAsOk(coursesAsResponse);
    }
}

public static class GetCoursesByUserIdEndpoint
{
    public static RouteGroupBuilder GetCoursesByUserIdGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/user/{userId:guid}",
                async (IMediator mediator, Guid userId) =>
                    await mediator.Send(new GetCoursesByUserIdQuery(userId)).ToGenericResultAsync())
            .WithName("GetCoursesByUserId")
            .MapToApiVersion(1, 0);

        return group;
    }
}