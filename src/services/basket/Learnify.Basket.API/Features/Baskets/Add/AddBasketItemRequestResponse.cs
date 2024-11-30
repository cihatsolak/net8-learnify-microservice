namespace Learnify.Basket.API.Features.Baskets.Add;

public sealed record AddBasketItemCommand(
    Guid CourseId,
    string CourseName,
    decimal CoursePrice,
    string ImageUrl) : IRequestResult;

public sealed class AddBasketItemCommandValidator : AbstractValidator<AddBasketItemCommand>
{
    public AddBasketItemCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.CourseName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.CoursePrice).GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");
    }
}
