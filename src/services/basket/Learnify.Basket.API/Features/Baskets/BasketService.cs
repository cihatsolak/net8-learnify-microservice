namespace Learnify.Basket.API.Features.Baskets;

public interface IBasketService
{
    Task<string> GetFromCacheAsync(CancellationToken cancellationToken);
    Task UpsertCacheAsync(Basket basket, CancellationToken cancellationToken);
}

public sealed class BasketService(IIdentityService identityService, IDistributedCache distributedCache) : IBasketService
{
    private string BasketCacheKey => string.Format(BasketConstant.BasketCacheKey, identityService.UserId);

    public Task<string> GetFromCacheAsync(CancellationToken cancellationToken)
    {
        return distributedCache.GetStringAsync(BasketCacheKey, cancellationToken);
    }

    public async Task UpsertCacheAsync(Basket basket, CancellationToken cancellationToken)
    {
        string basketAsJSON = JsonSerializer.Serialize(basket);
        await distributedCache.SetStringAsync(BasketCacheKey, basketAsJSON, token: cancellationToken);
    }
}
  