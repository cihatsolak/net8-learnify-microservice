namespace Learnify.Basket.API.Features;

public sealed record BasketDto(
    Guid UserId,
    List<BasketItemDto> Items
    );

public sealed record BasketItemDto(
            Guid Id,
            string Name,
            string ImageUrl,
            decimal Price,
            decimal? PriceByApplyDiscountRate);