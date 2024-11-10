namespace Learnify.Catalog.API.Features.Courses.Update;

public sealed record UpdateCourseCommand
    (
     Guid Id,
     string Name,
     string Description,
     decimal Price,
     string ImageUrl,
     Guid CategoryId
    ) : IRequestResult;

public sealed class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .MinimumLength(100).WithMessage("{PropertyName} must no exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(1000).WithMessage("{PropertyName} must no exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than 0.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}