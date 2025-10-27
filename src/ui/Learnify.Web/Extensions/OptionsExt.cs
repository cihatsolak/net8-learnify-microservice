namespace Learnify.Web.Extensions;

public static class OptionsServiceCollectionExtensions
{
    public static IServiceCollection AddOptionsExt(this IServiceCollection services)
    {
        services.AddOptionWithValidation<IdentityOption>();
        services.AddOptionWithValidation<GatewayOption>();
        services.AddOptionWithValidation<MicroserviceOption>();

        return services;
    }

    private static void AddOptionWithValidation<TOption>(this IServiceCollection services)
        where TOption : class
    {
        services.AddOptions<TOption>()
            .BindConfiguration(typeof(TOption).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<TOption>>().Value);
    }
}
