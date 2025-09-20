namespace Learnify.Payment.API.Features.Payments.GetStatus;

public record GetPaymentStatusRequest(string OrderCode) : IRequestResult<GetPaymentStatusResponse>;

public record GetPaymentStatusResponse(Guid PaymentId, bool IsPaid);
