
namespace Learnify.Order.Persistence.Repositories;

public sealed class OrderRepository(AppDbContext context) : GenericRepository<Guid, Domain.Entities.Order>(context), IOrderRepository
{
    public Task<List<Domain.Entities.Order>> GetByBuyerIdAsync(Guid buyerId)
    {
        return context.Orders
            .Include(order => order.OrderItems)
            .Where(x => x.BuyerId == buyerId)
            .OrderByDescending(x => x.Created)
            .ToListAsync();
    }

    public async Task SetStatusAsync(string orderCode, Guid paymentId, OrderStatus orderStatus, CancellationToken cancellationToken)
    {
       await  context.Orders.Where(x => x.Code == orderCode)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(order => order.Status, orderStatus)
                .SetProperty(order => order.PaymentId, paymentId),
                cancellationToken
            );
    }
}
