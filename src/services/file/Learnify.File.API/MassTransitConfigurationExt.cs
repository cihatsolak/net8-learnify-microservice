namespace Learnify.File.API;

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
            configurator.AddConsumer<UploadCoursePictureCommandConsumer>();

            configurator.UsingRabbitMq((context, cfg) =>
            {
                Uri hostUri = new($"rabbitmq://{busOptions.HostAddress}:{busOptions.Port}");
                cfg.Host(hostUri, host =>
                {
                    host.Username(busOptions.UserName);
                    host.Password(busOptions.Password);
                });

                //cfg.ConfigureEndpoints(context);
                cfg.ReceiveEndpoint("file-microservice.upload-course-picture-command.queue", e =>
                {
                    e.ConfigureConsumer<UploadCoursePictureCommandConsumer>(context);
                });
            });
        });

        return services;
    }
}
