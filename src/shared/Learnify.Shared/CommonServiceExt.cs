namespace Learnify.Shared;

public static class CommonServiceExt
{
    public static IServiceCollection AddCommonServiceExt(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());

        services.AddAutoMapper(Assembly.GetCallingAssembly());

        return services;
    }

    public static IServiceCollection AddOptionsExt<TOption>(this IServiceCollection services) where TOption : class, new()
    {
        services.AddOptions<TOption>()
                .BindConfiguration(typeof(TOption).Name)
                .ValidateDataAnnotations()
                .ValidateOnStart();

        return services;
    }
}
