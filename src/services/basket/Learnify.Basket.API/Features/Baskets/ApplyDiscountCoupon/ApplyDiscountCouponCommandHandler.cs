namespace Learnify.Basket.API.Features.Baskets.ApplyDiscountCoupon;

public sealed class ApplyDiscountCouponCommandHandler(ITokenService tokenService, IDistributedCache distributedCache)
        : IRequestHandler<ApplyDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(ApplyDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        Guid userId = tokenService.UserId;
        string cacheKey = string.Format(BasketConstant.BasketCacheKey, userId);

        string basketAsJson = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrWhiteSpace(basketAsJson))
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        Basket basket = JsonSerializer.Deserialize<Basket>(basketAsJson);
        if (basket.Items.Count == 0)
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        basket.ApplyNewDiscount(request.Coupon, request.DiscountRate);

        string updatedBasketAsJson = JsonSerializer.Serialize(basket);

        await distributedCache.SetStringAsync(cacheKey, updatedBasketAsJson, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}