namespace Learnify.Order.Application.Interfaces.Refit.Payments;

public record CreatePaymentResponse(Guid PaymentId, bool Status, string? ErrorMessage);
