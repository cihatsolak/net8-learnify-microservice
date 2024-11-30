namespace Learnify.Basket.API.Features.Baskets.Get;

public static class GetBasketQueryEndpoint
{
    public static RouteGroupBuilder GetBasketGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("/user", async (IMediator mediator) =>
                    await mediator.Send(new GetBasketQuery()).ToGenericResultAsync())
            .WithName("GetBasket")
            .MapToApiVersion(1, 0);

        return routeGroupBuilder;
    }
}