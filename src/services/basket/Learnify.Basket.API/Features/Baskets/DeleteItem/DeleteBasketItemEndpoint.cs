namespace Learnify.Basket.API.Features.Baskets.DeleteItem;

public static class DeleteBasketItemEndpoint
{
    public static RouteGroupBuilder DeleteBasketItemGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapDelete("/item/{id:guid}",async (Guid id, IMediator mediator) =>
                    await mediator.Send(new DeleteBasketItemCommand(id)).ToGenericResultAsync())
            .WithName("DeleteBasketItem")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<DeleteBasketItemCommandValidator>>();


        return routeGroupBuilder;
    }
}
