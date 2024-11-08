namespace Learnify.Catalog.API.Features.Categories.Create;

/// <summary>
/// Command to create a new category.
/// </summary>
/// <param name="Name">The name of the category.</param>
public sealed record CreateCategoryCommand(string Name) : IRequestResult<CreateCategoryResponse>;

/// <summary>
/// Response containing the ID of the newly created category.
/// </summary>
/// <param name="Id">The ID of the created category.</param>
public sealed record CreateCategoryResponse(Guid Id);

/// <summary>
/// Validator for the <see cref="CreateCategoryCommand"/>.
/// </summary>
public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCategoryCommandValidator"/> class.
    /// </summary>
    public CreateCategoryCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(4, 25).WithMessage("{PropertyName} must not exceed 50 characters.");
    }
}
