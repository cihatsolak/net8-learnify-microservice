namespace Learnify.Basket.API.Features.Baskets.RemoveBasketCoupon;

public class RemoveDiscountCouponCommandHandler(IBasketService basketService) : IRequestHandler<RemoveDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(RemoveDiscountCouponCommand request,
        CancellationToken cancellationToken)
    {
        string basketAsJSON = await basketService.GetFromCacheAsync(cancellationToken);
        if (string.IsNullOrEmpty(basketAsJSON))
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        Basket basket = JsonSerializer.Deserialize<Basket>(basketAsJSON)!;
        if (basket is null)
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        basket.ClearDiscount();

        await basketService.UpsertCacheAsync(basket, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}
