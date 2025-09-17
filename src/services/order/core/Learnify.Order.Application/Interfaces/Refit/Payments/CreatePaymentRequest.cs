namespace Learnify.Order.Application.Interfaces.Refit.Payments;

public record CreatePaymentRequest(
        string OrderCode,
        string CardNumber,
        string CardHolderName,
        string CardExpirationDate,
        string CardSecurityNumber,
        decimal Amount);