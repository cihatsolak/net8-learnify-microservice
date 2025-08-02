using Learnify.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Learnify.Shared.Extensions;

public static class AuthenticationExt
{
    public static IServiceCollection AddAuthenticationAndAuthorizationExt(this IServiceCollection services, IConfiguration configuration)
    {
        /*
         Sign
         Aud --> payment.api
         Issuer --> http://localhost:8080/realms/LearnifyTenant
         TTL (Token Life Time)
         */

        var identityOptions = configuration.GetRequiredSection(nameof(IdentityOptions)).Get<IdentityOptions>();

        services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = identityOptions.Authority;
                options.Audience = identityOptions.Audience;
                options.RequireHttpsMetadata = false; //keycloak https üzerinden ayağa kalkmadığı için kapadım.

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true
                };
            });

        services.AddAuthorization();

        return services;
    }
}
