namespace Learnify.Basket.API.Features.Baskets.Get;

public sealed class GetBasketQueryHandler(IDistributedCache distributedCache, ITokenService tokenService)
        : IRequestHandler<GetBasketQuery, ServiceResult<BasketDto>>
{
    public async Task<ServiceResult<BasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        Guid userId = tokenService.UserId;
        string cacheKey = string.Format(BasketConstant.BasketCacheKey, userId);

        string basketAsString = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrEmpty(basketAsString))
        {
            return ServiceResult<BasketDto>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        var basketDto = JsonSerializer.Deserialize<BasketDto>(basketAsString)!;
        if (basketDto is null)
        {
            return ServiceResult<BasketDto>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        return ServiceResult<BasketDto>.SuccessAsOk(basketDto);
    }
}