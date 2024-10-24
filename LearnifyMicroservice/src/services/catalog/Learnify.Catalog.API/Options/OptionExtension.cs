namespace Learnify.Catalog.API.Options;

public static class OptionExtension
{
    public static IServiceCollection AddOptionsExt<T>(this IServiceCollection services) where T : class, new()
    {
        services.AddOptions<T>()
                .BindConfiguration(typeof(T).Name)
                .ValidateDataAnnotations()
                .ValidateOnStart();

        return services;
    }
}
