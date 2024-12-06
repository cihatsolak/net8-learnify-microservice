namespace Learnify.Basket.API.Features.Baskets.RemoveBasketCoupon;

public static class RemoveDiscountCouponEndpoint
{
    public static RouteGroupBuilder RemoveDiscountCouponGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapDelete("/remove-discount-coupon",
                async (IMediator mediator) =>
                   {
                       await mediator.Send(new RemoveDiscountCouponCommand()).ToGenericResultAsync();
                   })
            .WithName("RemoveDiscountCoupon")
            .MapToApiVersion(1, 0);

        return routeGroupBuilder;
    }
}