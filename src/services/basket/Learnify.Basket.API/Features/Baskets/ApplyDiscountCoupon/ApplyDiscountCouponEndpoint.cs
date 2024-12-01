namespace Learnify.Basket.API.Features.Baskets.ApplyDiscountCoupon;

public static class ApplyDiscountCouponEndpoint
{
    public static RouteGroupBuilder ApplyDiscountCouponGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPut("/apply-discount-coupon",
                async (ApplyDiscountCouponCommand command, IMediator mediator) =>
                    await mediator.Send(command).ToGenericResultAsync())
            .WithName("ApplyDiscountCoupon")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<ApplyDiscountCouponCommandValidator>>();

        return routeGroupBuilder;
    }
}
