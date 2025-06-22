namespace Learnify.Payment.API.Features.Payments.GetAll;

public sealed record GetAllPaymentsByUserIdQuery : IRequestResult<List<GetAllPaymentsByUserIdResponse>>;

public record GetAllPaymentsByUserIdResponse(
       Guid Id,
       string OrderCode,
       string Amount,
       DateTime Created,
       PaymentStatus Status);

