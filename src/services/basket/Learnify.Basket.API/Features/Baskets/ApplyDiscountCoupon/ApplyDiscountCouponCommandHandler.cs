namespace Learnify.Basket.API.Features.Baskets.ApplyDiscountCoupon;

public sealed class ApplyDiscountCouponCommandHandler(IBasketService basketService) : IRequestHandler<ApplyDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(ApplyDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        string basketAsJSON = await basketService.GetFromCacheAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(basketAsJSON))
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        Basket basket = JsonSerializer.Deserialize<Basket>(basketAsJSON);
        if (basket.Items.Count == 0)
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        basket.ApplyNewDiscount(request.Coupon, request.DiscountRate);

        await basketService.UpsertCacheAsync(basket, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}