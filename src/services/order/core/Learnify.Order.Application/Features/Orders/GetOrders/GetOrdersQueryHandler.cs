namespace Learnify.Order.API.Features.GetOrders;

public sealed class GetOrdersQueryHandler(
    ITokenService tokenService,
    IOrderRepository orderRepository,
    IMapper mapper) : IRequestHandler<GetOrdersQuery, ServiceResult<List<GetOrdersResponse>>>
{
    public async Task<ServiceResult<List<GetOrdersResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetByBuyerIdAsync(tokenService.UserId);

        var response = orders.Select(order =>
            new GetOrdersResponse(
                order.Created, 
                order.TotalPrice,
                mapper.Map<List<OrderItemDto>>(order.OrderItems))
            ).ToList();

        return ServiceResult<List<GetOrdersResponse>>.SuccessAsOk(response);
    }
}