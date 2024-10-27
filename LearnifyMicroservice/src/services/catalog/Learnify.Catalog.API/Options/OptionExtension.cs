namespace Learnify.Catalog.API.Options;

public static class OptionExtension
{
    public static IServiceCollection AddOptionsExt<TOption>(this IServiceCollection services) where TOption : class, new()
    {
        services.AddOptions<TOption>()
                .BindConfiguration(typeof(TOption).Name)
                .ValidateDataAnnotations()
                .ValidateOnStart();

        return services;
    }
}