using FluentValidation;
using FluentValidation.AspNetCore;

namespace Learnify.Shared;

public static class CommonServivceExt
{
    public static IServiceCollection AddCommonServiceExt(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());

        return services;
    }
}
