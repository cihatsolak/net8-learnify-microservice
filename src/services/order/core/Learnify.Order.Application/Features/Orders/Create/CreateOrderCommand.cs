namespace Learnify.Order.Application.Features.Orders.Create;

#region Request
public sealed record CreateOrderCommand(float? DiscountRate, AddressDto Address, PaymentDto Payment, List<OrderItemDto> Items) : IRequestResult;

public sealed record AddressDto(string Province, string District, string Street, string ZipCode, string Line);

public sealed record PaymentDto(string CardNumber, string CardHolderName, string Expiration, string Cvv, decimal Amount);

public sealed record OrderItemDto(Guid ProductId, string ProductName, decimal UnitPrice);
#endregion
