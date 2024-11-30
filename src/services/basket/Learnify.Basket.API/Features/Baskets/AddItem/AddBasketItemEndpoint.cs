namespace Learnify.Basket.API.Features.Baskets.AddItem;

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
