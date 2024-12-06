namespace Learnify.Basket.API.Features.Baskets.AddItem;

public sealed class AddBasketItemCommandHandler(IDistributedCache distributedCache, ITokenService tokenService)
    : IRequestHandler<AddBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        Guid userId = tokenService.UserId;
        string cacheKey = string.Format(BasketConstant.BasketCacheKey, userId);

        Basket currentBasket;
        BasketItem newBasketItem = new(request.CourseId, request.CourseName, request.ImageUrl, request.CoursePrice, null);

        string basketAsString = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrEmpty(basketAsString))
        {
            currentBasket = new Basket(userId, [newBasketItem]);

            return await CreateCacheAsync(currentBasket, cacheKey, cancellationToken);
        }

        currentBasket = JsonSerializer.Deserialize<Basket>(basketAsString);
        currentBasket ??= new Basket(userId, [newBasketItem]);

        var existingBasketItemDto = currentBasket.Items.Find(basketItem => basketItem.Id == request.CourseId);
        if (existingBasketItemDto is not null)
        {
            currentBasket.Items.Remove(existingBasketItemDto);
        }

        currentBasket.Items.Add(newBasketItem);

        currentBasket.ApplyAvailableDiscount();

        return await CreateCacheAsync(currentBasket, cacheKey, cancellationToken);
    }

    private async Task<ServiceResult> CreateCacheAsync(Basket basketDto, string cacheKey, CancellationToken cancellationToken)
    {
        string basketAsString = JsonSerializer.Serialize(basketDto);
        await distributedCache.SetStringAsync(cacheKey, basketAsString, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}