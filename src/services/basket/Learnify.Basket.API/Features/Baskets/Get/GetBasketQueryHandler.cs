namespace Learnify.Basket.API.Features.Baskets.Get;

public sealed class GetBasketQueryHandler(
    IDistributedCache distributedCache, 
    ITokenService tokenService,
    IMapper mapper)
        : IRequestHandler<GetBasketQuery, ServiceResult<BasketResponse>>
{
    public async Task<ServiceResult<BasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        Guid userId = tokenService.UserId;
        string cacheKey = string.Format(BasketConstant.BasketCacheKey, userId);

        string basketAsString = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrEmpty(basketAsString))
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        var basket = JsonSerializer.Deserialize<Basket>(basketAsString)!;
        if (basket is null)
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        var basketResponse = mapper.Map<BasketResponse>(basket);

        return ServiceResult<BasketResponse>.SuccessAsOk(basketResponse);
    }
}