namespace Learnify.Order.Application.Interfaces.Refit;

public static class RefitServiceExtensions
{
    public static IServiceCollection AddRefitConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthenticatedHttpClientHandler>();
        services.AddScoped<ClientAuthenticatedHttpClientHandler>();

        services.AddOptions<IdentityOptions>()
               .BindConfiguration(nameof(IdentityOptions))
               .ValidateDataAnnotations()
               .ValidateOnStart();

        services.AddOptions<ClientSecretOptions>()
               .BindConfiguration(nameof(ClientSecretOptions))
               .ValidateDataAnnotations()
               .ValidateOnStart();

        var addressUrlOptions = configuration.GetSection(nameof(AddressUrlOptions)).Get<AddressUrlOptions>();

        services.AddRefitClient<IPaymentService>()
            .ConfigureHttpClient(cfg =>
            {
                cfg.BaseAddress = new Uri(addressUrlOptions?.PaymentService.BaseUrl 
                    ?? throw new InvalidOperationException("PaymentService BaseUrl is not configured."));
            })
            .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
            .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

        return services;
    }
}
