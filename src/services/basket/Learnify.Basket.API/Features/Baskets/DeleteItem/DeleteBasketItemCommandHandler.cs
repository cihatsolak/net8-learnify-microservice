namespace Learnify.Basket.API.Features.Baskets.DeleteItem;

public sealed class DeleteBasketItemCommandHandler(IDistributedCache distributedCache, ITokenService tokenService)
    : IRequestHandler<DeleteBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
    {
        Guid userId = tokenService.UserId;
        string cacheKey = string.Format(BasketConstant.BasketCacheKey, userId);

        string basketAsString = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrEmpty(basketAsString))
        {
            return ServiceResult.Error("Basket not found.", StatusCodes.Status404NotFound);
        }

        Basket currentBasket = JsonSerializer.Deserialize<Basket>(basketAsString);
        if (currentBasket is null)
        {
            return ServiceResult.Error("Basket not found.", StatusCodes.Status404NotFound);
        }

        var basketItemToDelete = currentBasket.Items.Find(basketItem => basketItem.Id == request.Id);
        if (basketItemToDelete is null)
        {
            return ServiceResult.Error("Basket item not found.", StatusCodes.Status400BadRequest);
        }

        currentBasket.Items.Remove(basketItemToDelete);
        await distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(currentBasket), cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }   
}