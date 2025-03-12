namespace Learnify.Order.API.Features.GetOrders;

public sealed record GetOrdersQuery : IRequestResult<List<GetOrdersResponse>>;

public sealed record GetOrdersResponse(DateTime Created, decimal TotalPrice, List<OrderItemDto> Items);