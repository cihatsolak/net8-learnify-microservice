using Microsoft.AspNetCore.Mvc;

namespace Learnify.Order.Application.Features.Orders.Create;

public sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IIdentityService identityService,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint,
    IPaymentService paymentService) : IRequestHandler<CreateOrderCommand, ServiceResult>
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

        var order = Domain.Entities.Order.CreateUnPaidOrder(identityService.UserId, request.DiscountRate, newAddress.Id);

        foreach (var orderItem in request.Items)
        {
            order.AddOrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.UnitPrice);
        }

        order.Address = newAddress;

        await orderRepository.AddAsync(order);
        await unitOfWork.CommitAsync(cancellationToken);

        //Ödeme servisi çağrılacak
        CreatePaymentRequest createPaymentRequest = new
            (
            order.Code,
            request.Payment.CardNumber,
            request.Payment.CardHolderName,
            request.Payment.Expiration,
            request.Payment.Cvv,
            order.TotalPrice
            );

        CreatePaymentResponse createPaymentResponse = await paymentService.CreateAsync(createPaymentRequest);
        if (!createPaymentResponse.Status)
        {
            return ServiceResult.Error("Payment failed", StatusCodes.Status400BadRequest);
        }

        order.SetPaidStatus(createPaymentResponse.PaymentId);

        orderRepository.Update(order);
        await unitOfWork.CommitAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderCreatedEvent(order.Id, identityService.UserId), cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}