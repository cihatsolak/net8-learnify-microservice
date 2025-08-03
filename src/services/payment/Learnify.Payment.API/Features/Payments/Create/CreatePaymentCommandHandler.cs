namespace Learnify.Payment.API.Features.Payments.Create;

public static class CreatePaymentCommandEndpoint
{
    public static RouteGroupBuilder CreatePaymentGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("",
                async ([FromBody] CreatePaymentCommand createPaymentCommand, IMediator mediator) =>
                await mediator.Send(createPaymentCommand).ToGenericResultAsync())
            .WithName("create")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(Policy.Password);

        return group;
    }
}

public sealed class CreatePaymentCommandHandler(AppDbContext appDbContext, IIdentityService identityService)
        : IRequestHandler<CreatePaymentCommand, ServiceResult<Guid>>
{
    public async Task<ServiceResult<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var (isSuccess, errorMessage) = await ExternalPaymentProcessAsync(
            request.CardNumber,
            request.CardHolderName,
            request.CardExpirationDate,
            request.CardSecurityNumber,
            request.Amount);

        if (!isSuccess)
        {
            return ServiceResult<Guid>.Error("Payment Failed", errorMessage, StatusCodes.Status400BadRequest);
        }


        var userId = identityService.UserId;
        var newPayment = new Repositories.Payment(userId, request.OrderCode, request.Amount);
        newPayment.SetStatus(PaymentStatus.Success);

        appDbContext.Payments.Add(newPayment);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<Guid>.SuccessAsOk(newPayment.Id);
    }


    private async static Task<(bool isSuccess, string? errorMessage)> ExternalPaymentProcessAsync(
        string cardNumber,
        string cardHolderName,
        string cardExpirationDate,
        string cardSecurityNumber,
        decimal amount)
    {
        // Simulate external payment processing logic
        await Task.Delay(1000); // Simulating a delay for the external service call
        return (true, null); // Assume the payment was successful

        //return (false,"Payment failed due to insufficient funds."); // Simulate a failure case
    }
}
