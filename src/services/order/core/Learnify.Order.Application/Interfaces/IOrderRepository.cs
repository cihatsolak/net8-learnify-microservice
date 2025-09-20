namespace Learnify.Order.Application.Interfaces;

public interface IOrderRepository : IGenericRepository<Guid, Domain.Entities.Order>
{
    Task<List<Domain.Entities.Order>> GetByBuyerIdAsync(Guid buyerId);
    Task SetStatusAsync(string orderCode, Guid paymentId, OrderStatus orderStatus, CancellationToken cancellationToken);
}
