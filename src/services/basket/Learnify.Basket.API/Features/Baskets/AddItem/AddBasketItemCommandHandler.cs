namespace Learnify.Basket.API.Features.Baskets.AddItem;

public sealed class AddBasketItemCommandHandler(ITokenService tokenService, IBasketService basketService)
    : IRequestHandler<AddBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        Guid userId = tokenService.UserId;
        string basketAsJSON = await basketService.GetFromCacheAsync(cancellationToken);

        Basket currentBasket;
        BasketItem newBasketItem = new(request.CourseId, request.CourseName, request.ImageUrl, request.CoursePrice, null);

        if (string.IsNullOrEmpty(basketAsJSON))
        {
            currentBasket = new Basket(userId, [newBasketItem]);
            await basketService.UpsertCacheAsync(currentBasket, cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }

        currentBasket = JsonSerializer.Deserialize<Basket>(basketAsJSON);
        currentBasket ??= new Basket(userId, [newBasketItem]);

        var existingBasketItemDto = currentBasket.Items.Find(basketItem => basketItem.Id == request.CourseId);
        if (existingBasketItemDto is not null)
        {
            currentBasket.Items.Remove(existingBasketItemDto);
        }

        currentBasket.Items.Add(newBasketItem);

        currentBasket.ApplyAvailableDiscount();

        await basketService.UpsertCacheAsync(currentBasket, cancellationToken);
        return ServiceResult.SuccessAsNoContent();
    }
}