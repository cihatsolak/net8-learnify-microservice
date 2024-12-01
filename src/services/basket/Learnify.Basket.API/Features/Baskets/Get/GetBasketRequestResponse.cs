namespace Learnify.Basket.API.Features.Baskets.Get;

public sealed record GetBasketQuery : IRequestResult<BasketResponse>;

public sealed record BasketResponse
{
    public List<BasketItemResponse> Items { get; set; }
}

public sealed record BasketItemResponse(
            Guid Id,
            string Name,
            string ImageUrl,
            decimal Price,
            decimal? PriceByApplyDiscountRate);