namespace Learnify.Discount.API.Features.Discounts;

public static class DiscountEndpointExt
{
    public static void AddDiscountGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/discounts")
            .WithTags("discounts")
            .WithApiVersionSet(apiVersionSet)
            .CreateDiscountGroupItemEndpoint()
            .GetDiscountByCodeGroupItemEndpoint()
            .RequireAuthorization();
    }
}