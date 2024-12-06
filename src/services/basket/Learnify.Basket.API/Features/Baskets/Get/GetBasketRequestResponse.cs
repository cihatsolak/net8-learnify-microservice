namespace Learnify.Basket.API.Features.Baskets.Get;

public sealed record GetBasketQuery : IRequestResult<BasketResponse>;

public sealed record BasketResponse
{
    [JsonIgnore] public bool IsApplyDiscount => DiscountRate is > 0 && !string.IsNullOrEmpty(Coupon);

    [JsonIgnore] public decimal TotalPrice => Items.Sum(basketItem => basketItem.Price);

    [JsonIgnore]
    public decimal? TotalPriceWithAppliedDiscount =>
        !IsApplyDiscount ? null : Items.Sum(basketItem => basketItem.PriceByApplyDiscountRate);

    public float? DiscountRate { get; set; }
    public string Coupon { get; set; }

    public List<BasketItemResponse> Items { get; set; }
}

public sealed record BasketItemResponse(
            Guid Id,
            string Name,
            string ImageUrl,
            decimal Price,
            decimal? PriceByApplyDiscountRate);