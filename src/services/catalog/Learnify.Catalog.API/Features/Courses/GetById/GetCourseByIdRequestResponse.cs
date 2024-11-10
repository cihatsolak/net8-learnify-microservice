namespace Learnify.Catalog.API.Features.Courses.GetById;

public sealed record GetCourseByIdQuery(Guid Id) : IRequestResult<CourseResponse>;

public sealed class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
{
    public GetCourseByIdQueryValidator()
    {
        RuleFor(course => course.Id)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");
    }
}