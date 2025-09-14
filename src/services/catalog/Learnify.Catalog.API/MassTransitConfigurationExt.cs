namespace Learnify.Catalog.API;

public static class MassTransitConfigurationExt
{
    public static IServiceCollection AddMassTransitExt(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        var busOptions = configuration
            .GetSection(nameof(BusOptions))
            .Get<BusOptions>()
                ?? throw new InvalidOperationException("BusOptions missing in configuration.");

        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<CoursePictureUploadedEventConsumer>();

            configurator.UsingRabbitMq((context, cfg) =>
            {
                Uri hostUri = new($"rabbitmq://{busOptions.HostAddress}:{busOptions.Port}");
                cfg.Host(hostUri, host =>
                {
                    host.Username(busOptions.UserName);
                    host.Password(busOptions.Password);
                });

                //cfg.ConfigureEndpoints(context);
                cfg.ReceiveEndpoint("catalog-microservice.course-picture-uploaded.queue", e =>
                {
                    e.ConfigureConsumer<CoursePictureUploadedEventConsumer>(context);
                });
            });
        });

        return services;
    }
}
