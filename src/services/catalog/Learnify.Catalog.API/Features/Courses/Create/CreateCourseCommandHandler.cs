namespace Learnify.Catalog.API.Features.Courses.Create;

public static class CreateCourseCommandEndpoint
{
    public static RouteGroupBuilder CreateCourseGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/", async (CreateCourseCommand command, IMediator mediator)
            => await mediator.Send(command).ToGenericResultAsync())
            .WithName("CreateCourse")
            .AddEndpointFilter<ValidationFilter<CreateCourseCommandValidator>>()
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return routeGroupBuilder;
    }
}

public sealed class CreateCourseCommandHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<CreateCourseCommand, ServiceResult<Guid>>
{
    public async Task<ServiceResult<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        bool isCategoryExists = await context.Categories.AnyAsync(category => category.Id == request.CategoryId, cancellationToken);
        if (!isCategoryExists)
        {
            return ServiceResult<Guid>.Error(
                "Category not found.",
                $"The category with id({request.CategoryId}) was not found.",
                StatusCodes.Status404NotFound);
        }

        var isCourseExists = await context.Courses.AnyAsync(course => course.Name == request.Name, cancellationToken);
        if (isCourseExists)
        {
            return ServiceResult<Guid>.Error(
                "Course already exists.",
                $"The course with name ({request.Name}) already exists.",
                StatusCodes.Status400BadRequest);
        }

        var course = mapper.Map<Course>(request);
        course.Feature = new()
        {
            Duration = 10, //calculate from video length
            EducatorFullName = "Cihat Solak", //token user
            Rating = 0 //calculate from user ratings
        };

        await context.Courses.AddAsync(course, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<Guid>.SuccessAsCreated(course.Id, $"/api/courses/{course.Id}");
    }
}
