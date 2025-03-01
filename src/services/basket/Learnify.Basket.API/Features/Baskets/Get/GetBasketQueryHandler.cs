﻿namespace Learnify.Basket.API.Features.Baskets.Get;

public sealed class GetBasketQueryHandler(
    IBasketService basketService,
    IMapper mapper) : IRequestHandler<GetBasketQuery, ServiceResult<BasketResponse>>
{
    public async Task<ServiceResult<BasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        string basketAsJSON = await basketService.GetFromCacheAsync(cancellationToken);
        if (string.IsNullOrEmpty(basketAsJSON))
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        Basket basket = JsonSerializer.Deserialize<Basket>(basketAsJSON)!;
        if (basket is null)
        {
            return ServiceResult<BasketResponse>.Error("Basket not found", StatusCodes.Status404NotFound);
        }

        BasketResponse basketResponse = mapper.Map<BasketResponse>(basket);

        return ServiceResult<BasketResponse>.SuccessAsOk(basketResponse);
    }
}