namespace Learnify.Basket.API.Features.Baskets.RemoveBasketCoupon;

public sealed record RemoveDiscountCouponCommand : IRequestResult;

public class RemoveDiscountCouponCommandHandler(
        ITokenService tokenService,
        IDistributedCache distributedCache) : IRequestHandler<RemoveDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(RemoveDiscountCouponCommand request,
        CancellationToken cancellationToken)
    {
        Guid userId = tokenService.UserId;
        string cacheKey = string.Format(BasketConstant.BasketCacheKey, userId););

        string basketAsJSON = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
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

        await distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}
