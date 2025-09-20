namespace Learnify.Order.Application.Interfaces.Refit.Payments;

public record PaymentStatusResponse(Guid PaymentId, bool IsPaid);
