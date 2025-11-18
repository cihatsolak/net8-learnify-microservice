namespace Learnify.Payment.API.Features.Payments;

public static class PaymentEndpointExt
{
    public static void AddPaymentGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/payments")
           .WithTags("payments")
           .WithApiVersionSet(apiVersionSet)
           .CreatePaymentGroupItemEndpoint()
           .GetAllPaymentsByUserIdGroupItemEndpoint()
           .GetPaymentStatusGroupItemEndpoint();
    }
}
