namespace Learnify.Payment.API.Features.Payments.GetStatus;

public static class GetPaymentStatusQueryEndpoint
{
    public static RouteGroupBuilder GetPaymentStatusGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/status/{orderCode}",
                async ([FromServices] IMediator mediator, string orderCode) =>
                (await mediator.Send(new GetAllPaymentsByUserIdQuery()).ToGenericResultAsync()))
            .WithName("GetPaymentStatus")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("ClientCredential");

        return group;
    }
}

public sealed class GetPaymentStatusQueryHandler(AppDbContext context)
        : IRequestHandler<GetPaymentStatusRequest, ServiceResult<GetPaymentStatusResponse>>
{
    public async Task<ServiceResult<GetPaymentStatusResponse>> Handle(GetPaymentStatusRequest request,
        CancellationToken cancellationToken)
    {
        var payment = await context.Payments.FirstOrDefaultAsync(payment => payment.OrderCode == request.OrderCode, cancellationToken);
        if (payment is null)
        {
            return ServiceResult<GetPaymentStatusResponse>.SuccessAsOk(new GetPaymentStatusResponse(Guid.Empty, false));
        }

        return ServiceResult<GetPaymentStatusResponse>.SuccessAsOk(
            new GetPaymentStatusResponse(payment.Id, payment.Status == PaymentStatus.Success));
    }
}
