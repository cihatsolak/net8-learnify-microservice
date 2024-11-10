namespace Learnify.Catalog.API.Features.Categories.GetById;

public sealed record GetCategoryByIdRequest(Guid Id) : IRequestResult<CategoryResponse>;

public sealed class GetCategoryByIdValidator : AbstractValidator<GetCategoryByIdRequest>
{
    public GetCategoryByIdValidator()
    {
        RuleFor(category => category.Id)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");
    }
}