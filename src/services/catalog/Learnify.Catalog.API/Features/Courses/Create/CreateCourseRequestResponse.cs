namespace Learnify.Catalog.API.Features.Courses.Create;

public sealed record CreateCourseCommand(string Name, 
                                         string Description, 
                                         decimal Price, 
                                         IFormFile Picture,
                                         Guid CategoryId)
                                         : IRequestResult<Guid>;

public sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
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