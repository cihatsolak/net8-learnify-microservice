namespace Learnify.Catalog.API.Features.Courses.Create;

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
                $"The course with name({request.Name}) already exists.",
                StatusCodes.Status400BadRequest);
        }

        var course = mapper.Map<Course>(request);

        await context.Courses.AddAsync(course, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<Guid>.SuccessAsCreated(course.Id, $"/api/courses/{course.Id}");
    }
}
