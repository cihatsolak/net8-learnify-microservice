namespace Learnify.Basket.API.Features;

public sealed record BasketDto
{
    public BasketDto()
    {
    }

    public BasketDto(Guid userId, List<BasketItemDto> items) 
    {
        UserId = userId;
        Items = items;
    }

    [JsonIgnore] public Guid UserId { get; init; }
    public List<BasketItemDto> Items { get; set; }
}


public sealed record BasketItemDto(
            Guid Id,
            string Name,
            string ImageUrl,
            decimal Price,
            decimal? PriceByApplyDiscountRate);