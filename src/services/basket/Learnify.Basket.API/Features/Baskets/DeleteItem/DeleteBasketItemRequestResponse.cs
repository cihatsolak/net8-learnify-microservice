namespace Learnify.Basket.API.Features.Baskets.DeleteItem;

public record DeleteBasketItemCommand(Guid Id) : IRequestResult;

public sealed class DeleteBasketItemCommandValidator : AbstractValidator<DeleteBasketItemCommand>
{
    public DeleteBasketItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required.");
    }
}