namespace Learnify.Discount.API.Features.Discounts.GetDiscountByCode;

public sealed class GetDiscountByCodeQueryHandler(AppDbContext context)
    : IRequestHandler<GetDiscountByCodeQuery, ServiceResult<GetDiscountByCodeQueryResponse>>
{
    public async Task<ServiceResult<GetDiscountByCodeQueryResponse>> Handle(GetDiscountByCodeQuery request, CancellationToken cancellationToken)
    {
        var hasDiscount = await context.Discounts.SingleOrDefaultAsync(disccount => disccount.Code == request.Code, cancellationToken);
        if (hasDiscount is null)
        {
            return ServiceResult<GetDiscountByCodeQueryResponse>.Error("Discount not found", StatusCodes.Status404NotFound);
        }

        if (hasDiscount.Expired < DateTime.Now)
        {
            return ServiceResult<GetDiscountByCodeQueryResponse>.Error("Discount is expired", StatusCodes.Status400BadRequest);
        }

        return ServiceResult<GetDiscountByCodeQueryResponse>.SuccessAsOk(new GetDiscountByCodeQueryResponse(hasDiscount.Code, hasDiscount.Rate));
    }
}

public static class GetDiscountByCodeEndpoint
{
    public static RouteGroupBuilder GetDiscountByCodeGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{code:length(10)}",
                async (string code, IMediator mediator) =>
                    await mediator.Send(new GetDiscountByCodeQuery(code)).ToGenericResultAsync())
            .WithName("GetDiscountByCode")
            .MapToApiVersion(1, 0)
            .Produces<GetDiscountByCodeQueryResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return group;
    }
}
