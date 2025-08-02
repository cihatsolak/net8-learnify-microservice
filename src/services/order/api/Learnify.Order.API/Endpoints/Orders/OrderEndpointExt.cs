namespace Learnify.Order.API.Endpoints.CreateOrder;

public static class OrderEndpointExt
{
    public static void AddOrderGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/orders")
            .WithTags("Orders")
            .WithApiVersionSet(apiVersionSet)
            .CreateOrderGroupItemEndpoint()
            .GetOrdersGroupItemEndpoint()
            .RequireAuthorization();
    }
}