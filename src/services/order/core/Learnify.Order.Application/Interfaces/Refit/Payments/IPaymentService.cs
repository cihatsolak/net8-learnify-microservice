namespace Learnify.Order.Application.Interfaces.Refit.Payments;

public interface IPaymentService
{
    [Post("/api/v1/payments")]
    Task<CreatePaymentResponse> CreateAsync(CreatePaymentRequest request);

    [Post("/api/v1/payments/status/{orderCode}")]
    Task<PaymentStatusResponse> GetStatusAsync(string orderCode, CancellationToken cancellationToken);
}
