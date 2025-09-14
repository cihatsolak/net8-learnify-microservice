namespace Learnify.Basket.API.Features.Baskets;

public interface IBasketService
{
    Task<string> GetFromCacheAsync(CancellationToken cancellationToken);
    Task UpsertCacheAsync(Basket basket, CancellationToken cancellationToken);
    Task DeleteBasketAsync(Guid userId);
}

public sealed class BasketService(IIdentityService identityService, IDistributedCache distributedCache) : IBasketService
{
    private string BasketCacheKey => string.Format(BasketConstant.BasketCacheKey, identityService.UserId);
    private string BasketCacheKeyByUserId(Guid userId) => string.Format(BasketConstant.BasketCacheKey, userId);

    public Task<string> GetFromCacheAsync(CancellationToken cancellationToken)
    {
        return distributedCache.GetStringAsync(BasketCacheKey, cancellationToken);
    }

    public async Task UpsertCacheAsync(Basket basket, CancellationToken cancellationToken)
    {
        string basketAsJSON = JsonSerializer.Serialize(basket);
        await distributedCache.SetStringAsync(BasketCacheKey, basketAsJSON, token: cancellationToken);
    }

    public async Task DeleteBasketAsync(Guid userId)
    {
        await distributedCache.RemoveAsync(BasketCacheKeyByUserId(userId));
    }
}
