namespace Learnify.Order.Application.BackgroundServices;

public sealed class CheckPaymentStatusOrderBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        while (!stoppingToken.IsCancellationRequested)
        {
            List<string> orderCodes =
                [.. orderRepository.Where(x => x.Status == OrderStatus.WaitingForPayment).Select(order => order.Code)];

            foreach (var orderCode in orderCodes)
            {
                var paymentStatusResponse = await paymentService.GetStatusAsync(orderCode, stoppingToken);
                if (paymentStatusResponse.IsPaid)
                {
                    await orderRepository.SetStatusAsync(
                        orderCode, 
                        paymentStatusResponse.PaymentId, 
                        OrderStatus.Paid, 
                        stoppingToken);
                }
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}
