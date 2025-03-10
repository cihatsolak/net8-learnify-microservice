namespace Learnify.Order.Application.Features.Orders.Create;

public sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    ITokenService tokenService,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Items.Count == 0)
            return ServiceResult.Error("Order items not found", "Order must have at least one item", StatusCodes.Status400BadRequest);

        Address newAddress = new()
        {
            Province = request.Address.Province,
            District = request.Address.District,
            Street = request.Address.Street,
            ZipCode = request.Address.ZipCode,
            Line = request.Address.Line
        };

        var order = Domain.Entities.Order.CreateUnPaidOrder(tokenService.UserId, request.DiscountRate, newAddress.Id);

        foreach (var orderItem in request.Items)
        {
            order.AddOrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.UnitPrice);
        }

        order.Address = newAddress;

        await orderRepository.AddAsync(order);
        await unitOfWork.CommitAsync(cancellationToken);

        var paymentId = Guid.Empty;


        //Payment işlemleri yapılacak

        order.SetPaidStatus(paymentId);

        orderRepository.Update(order);
        await unitOfWork.CommitAsync(cancellationToken);


        return ServiceResult.SuccessAsNoContent();
    }
}