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
                options.Authority = identityOptions.Address;
                options.Audience = identityOptions.Audience;
                options.RequireHttpsMetadata = false; //keycloak https üzerinden ayağa kalkmadığı için kapadım.

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    RoleClaimType = "roles",
                    NameClaimType = "preferred_username"
                };
            })
            .AddJwtBearer("ClientCredantialSchema", options =>
            {
                options.Authority = identityOptions.Address;
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

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policy.ClientCredential, policy =>
            {
                policy.AuthenticationSchemes.Add("ClientCredantialSchema");
                policy.RequireClaim("client_id");
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy(Policy.Password, policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireClaim(ClaimTypes.Email);
                policy.RequireAuthenticatedUser();

            });
        });

        return services;
    }
}
