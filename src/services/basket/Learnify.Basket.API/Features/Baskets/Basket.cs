namespace Learnify.Basket.API.Features.Baskets;

public sealed class Basket
{
    public Guid UserId { get; set; }
    public List<BasketItem> Items { get; set; } = [];
    public float? DiscountRate { get; set; }
    public string Coupon { get; set; }

    [JsonIgnore] public bool IsApplyDiscount => DiscountRate is > 0 && !string.IsNullOrEmpty(Coupon);

    [JsonIgnore] public decimal TotalPrice => Items.Sum(basketItem => basketItem.Price);

    [JsonIgnore]
    public decimal? TotalPriceWithAppliedDiscount =>
        !IsApplyDiscount ? null : Items.Sum(basketItem => basketItem.PriceByApplyDiscountRate);

    public Basket()
    {
    }

    public Basket(Guid userId, List<BasketItem> items)
    {
        UserId = userId;
        Items = items;
    }

    public void ApplyNewDiscount(string coupon, float discountRate)
    {
        Coupon = coupon;
        DiscountRate = discountRate;

        foreach (var basket in Items)
        {
            //Eğer indirim oranı %20 ise, bu 1 - 0.20 = 0.80 olur.
            basket.PriceByApplyDiscountRate = basket.Price * (decimal)(1 - discountRate);
        }
    }

    public void ApplyAvailableDiscount()
    {
        if (!IsApplyDiscount)
        {
            return;
        }

        foreach (var basket in Items)
        {
            //Eğer indirim oranı %20 ise, bu 1 - 0.20 = 0.80 olur.
            basket.PriceByApplyDiscountRate = basket.Price * (decimal)(1 - DiscountRate);
        }
    }

    public void ClearDiscount()
    {
        DiscountRate = null;
        Coupon = null;
        foreach (var basket in Items)
        {
            basket.PriceByApplyDiscountRate = null;
        }
    }
}

public sealed record BasketItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? PriceByApplyDiscountRate { get; set; }

    public BasketItem()
    {
    }

    public BasketItem(Guid id, string name, string imageUrl, decimal price, decimal? priceByApplyDiscountRate)
    {
        Id = id;
        Name = name;
        ImageUrl = imageUrl;
        Price = price;
        PriceByApplyDiscountRate = priceByApplyDiscountRate;
    }
}