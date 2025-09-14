namespace Learnify.Discount.API.Consumers;

public sealed class OrderCreatedEventConsumer(IServiceProvider serviceProvider) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        using var scope = serviceProvider.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var discount = new Features.Discounts.Discount()
        {
            Id = NewId.NextSequentialGuid(),
            Code = DiscountCodeGenerator.Generate(10),
            Created = DateTime.Now,
            Rate = 0.1f,
            Expired = DateTime.Now.AddMonths(1),
            UserId = context.Message.UserId
        };

        await dbContext.Discounts.AddAsync(discount);
        await dbContext.SaveChangesAsync();
    }
}
