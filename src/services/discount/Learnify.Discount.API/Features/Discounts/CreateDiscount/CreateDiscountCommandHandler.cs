namespace Learnify.Discount.API.Features.Discounts.CreateDiscount;

public sealed class CreateDiscountCommandHandler(AppDbContext context) : IRequestHandler<CreateDiscountCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var hasCodeForUser = await context.Discounts.AnyAsync(x => x.UserId.ToString() == request.UserId.ToString() && x.Code == request.Code, cancellationToken: cancellationToken);


        if (hasCodeForUser)
        {
            return ServiceResult.Error("Discount code already exists for this user", StatusCodes.Status400BadRequest);
        }


        Discount discount = new()
        {
            Id = NewId.NextSequentialGuid(),
            Code = request.Code,
            Created = DateTime.Now,
            Rate = request.Rate,
            Expired = request.Expired,
            UserId = request.UserId
        };

        await context.Discounts.AddAsync(discount, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}

public static class CreateDiscountCommandEndpoint
{
    public static RouteGroupBuilder CreateDiscountGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (CreateDiscountCommand command, IMediator mediator) =>
                await mediator.Send(command).ToGenericResultAsync())
            .WithName("CreateDiscount")
            .MapToApiVersion(1, 0)
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateDiscountCommandValidator>>();

        return group;
    }
}