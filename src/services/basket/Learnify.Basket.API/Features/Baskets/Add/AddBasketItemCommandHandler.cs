namespace Learnify.Basket.API.Features.Baskets.Add;

public static class AddBasketItemEndpoint
{
    public static RouteGroupBuilder AddBasketItemGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/item",
                async (AddBasketItemCommand command, IMediator mediator) =>
                    await mediator.Send(command).ToGenericResultAsync())
            .WithName("AddBasketItem")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<AddBasketItemCommandValidator>>();

        return routeGroupBuilder;
    }
}

public sealed class AddBasketItemCommandHandler(IDistributedCache distributedCache)
    : IRequestHandler<AddBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        Guid userId = Guid.NewGuid();
        string cacheKey = string.Format(BasketConstant.BasketCacheKey, userId);

        BasketDto currentBasket;
        BasketItemDto newBasketItem = new(request.CourseId, request.CourseName, request.ImageUrl, request.CoursePrice, null);

        string basketAsString = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrEmpty(basketAsString))
        {
            currentBasket = new BasketDto(userId, [newBasketItem]);

            return await CreateCacheAsync(currentBasket, cacheKey, cancellationToken);
        }

        currentBasket = JsonSerializer.Deserialize<BasketDto>(basketAsString);
        currentBasket ??= new BasketDto(userId, [newBasketItem]);

        var existingBasketItemDto = currentBasket.Items.Find(basketItem => basketItem.Id == request.CourseId);
        if (existingBasketItemDto is not null)
        {
            currentBasket.Items.Remove(existingBasketItemDto);
        }

        currentBasket.Items.Add(newBasketItem);

        return await CreateCacheAsync(currentBasket, cacheKey, cancellationToken);
    }

    private async Task<ServiceResult> CreateCacheAsync(BasketDto basketDto, string cacheKey, CancellationToken cancellationToken)
    {
        string basketAsString = JsonSerializer.Serialize(basketDto);
        await distributedCache.SetStringAsync(cacheKey, basketAsString, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}