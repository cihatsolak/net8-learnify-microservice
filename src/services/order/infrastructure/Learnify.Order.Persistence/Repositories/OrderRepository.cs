namespace Learnify.Order.Persistence.Repositories;

public sealed class OrderRepository(AppDbContext context) : GenericRepository<Guid, Domain.Entities.Order>(context), IOrderRepository
{
}
