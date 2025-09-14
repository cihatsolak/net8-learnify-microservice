namespace Learnify.Bus;

public static class MassTransitConfigurationExt
{
    public static IServiceCollection AddCommonMassTransitExt(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        var busOptions = configuration
            .GetSection(nameof(BusOptions))
            .Get<BusOptions>()
                ?? throw new InvalidOperationException("BusOptions missing in configuration.");

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                Uri hostUri = new($"rabbitmq://{busOptions.HostAddress}:{busOptions.Port}");
                cfg.Host(hostUri, host =>
                {
                    host.Username(busOptions.UserName);
                    host.Password(busOptions.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
