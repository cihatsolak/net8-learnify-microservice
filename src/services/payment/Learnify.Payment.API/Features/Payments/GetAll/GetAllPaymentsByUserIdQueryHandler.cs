namespace Learnify.Payment.API.Features.Payments.GetAll;

public static class GetAllPaymentsByUserIdEndpoint
{
    public static RouteGroupBuilder GetAllPaymentsByUserIdGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("",
                async (IMediator mediator) =>
                    await mediator.Send(new GetAllPaymentsByUserIdQuery()).ToGenericResultAsync())
            .WithName("get-all-payments-by-userid")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return group;
    }
}

public class GetAllPaymentsByUserIdQueryHandler(AppDbContext context, ITokenService tokenService)
        : IRequestHandler<GetAllPaymentsByUserIdQuery, ServiceResult<List<GetAllPaymentsByUserIdResponse>>>
{
    public async Task<ServiceResult<List<GetAllPaymentsByUserIdResponse>>> Handle(
        GetAllPaymentsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = tokenService.UserId;

        var payments = await context.Payments
            .Where(x => x.UserId == userId)
            .Select(x => new GetAllPaymentsByUserIdResponse(
                x.Id,
                x.OrderCode,
                x.Amount.ToString("C"), // Format as currency
                x.Created,
                x.Status))
            .ToListAsync(cancellationToken);


        return ServiceResult<List<GetAllPaymentsByUserIdResponse>>.SuccessAsOk(payments);
    }
}
