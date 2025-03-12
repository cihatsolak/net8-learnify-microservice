namespace Learnify.Order.Application.Features.Orders;
public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
    }
}