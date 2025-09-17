namespace Learnify.Shared.Options;

public sealed record AddressUrlOptions
{
    public PaymentServiceOptions PaymentService { get; init; }
}

public sealed record PaymentServiceOptions
{
    public required string BaseUrl { get; init; }
    public required string CreatePayment { get; init; }
    public required string GetAllPayments { get; init; }
}
