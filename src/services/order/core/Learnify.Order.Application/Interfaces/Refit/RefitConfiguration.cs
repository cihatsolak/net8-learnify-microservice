namespace Learnify.Order.Application.Interfaces.Refit;

public static class RefitConfiguration
{
    public static IServiceCollection AddRefitConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var addressUrlOptions = configuration.GetSection(nameof(AddressUrlOptions)).Get<AddressUrlOptions>();

        services.AddRefitClient<IPaymentService>()
            .ConfigureHttpClient(cfg =>
            {
                cfg.BaseAddress = new Uri(addressUrlOptions?.PaymentService.BaseUrl 
                    ?? throw new InvalidOperationException("PaymentService BaseUrl is not configured."));
            });

        return services;
    }
}
