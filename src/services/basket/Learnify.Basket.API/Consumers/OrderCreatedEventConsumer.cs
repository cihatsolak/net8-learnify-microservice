namespace Learnify.Basket.API.Consumers;

public sealed class OrderCreatedEventConsumer(IServiceProvider serviceProvider) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        using var scope = serviceProvider.CreateScope();
        IBasketService basketService = scope.ServiceProvider.GetRequiredService<IBasketService>();

        await basketService.DeleteBasketAsync(context.Message.UserId);
    }
}
