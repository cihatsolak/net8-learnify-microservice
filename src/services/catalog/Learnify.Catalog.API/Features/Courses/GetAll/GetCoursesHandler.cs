namespace Learnify.Catalog.API.Features.Courses.GetAll;

public static class GetCoursesEndpoint
{
    public static RouteGroupBuilder GetCoursesGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetCoursesQuery()).ToGenericResultAsync();
        })
        .WithName("Courses")
        .MapToApiVersion(1, 0);

        return routeGroupBuilder;
    }
}

public sealed class GetCoursesHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetCoursesQuery, ServiceResult<List<CourseResponse>>>
{
    public async Task<ServiceResult<List<CourseResponse>>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await context.Courses.AsNoTracking().ToListAsync(cancellationToken);
        var categories = await context.Categories.AsNoTracking().ToListAsync(cancellationToken);

        foreach (var course in courses)
        {
            course.Category = categories.Find(category => category.Id == course.CategoryId);
        }

        return ServiceResult<List<CourseResponse>>.SuccessAsOk(mapper.Map<List<CourseResponse>>(courses));
    }
}