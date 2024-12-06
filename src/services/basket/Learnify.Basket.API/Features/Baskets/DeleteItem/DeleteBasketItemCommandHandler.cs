namespace Learnify.Basket.API.Features.Baskets.DeleteItem;

public sealed class DeleteBasketItemCommandHandler(IBasketService basketService) : IRequestHandler<DeleteBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
    {
        string basketAsJSON = await basketService.GetFromCacheAsync(cancellationToken);
        if (string.IsNullOrEmpty(basketAsJSON))
        {
            return ServiceResult.Error("Basket not found.", StatusCodes.Status404NotFound);
        }

        Basket basket = JsonSerializer.Deserialize<Basket>(basketAsJSON);
        if (basket is null)
        {
            return ServiceResult.Error("Basket not found.", StatusCodes.Status404NotFound);
        }

        BasketItem basketItemToDelete = basket.Items.Find(basketItem => basketItem.Id == request.Id);
        if (basketItemToDelete is null)
        {
            return ServiceResult.Error("Basket item not found.", StatusCodes.Status400BadRequest);
        }

        basket.Items.Remove(basketItemToDelete);
        await basketService.UpsertCacheAsync(basket, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}