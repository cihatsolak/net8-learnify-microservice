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
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.NameIdentifier
                };

                // AutomaticRefreshInterval: otomatik aralıkla metadata/JWKS yenileme (ör. 24saat)
                options.AutomaticRefreshInterval = TimeSpan.FromHours(24);

                // RefreshInterval: RequestRefresh() çağrıldıktan sonra beklenen min süre (ör. 30s)
                options.RefreshInterval = TimeSpan.FromSeconds(30);
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

        services.AddAuthorizationBuilder()
            .AddPolicy(Policy.Instructor, policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Email);
                policy.RequireRole(ClaimTypes.Role, "instructor");
            })
            .AddPolicy(Policy.Password, policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Email);

            })
            .AddPolicy(Policy.ClientCredential, policy =>
            {
                policy.AuthenticationSchemes.Add("ClientCredantialSchema");
                policy.RequireAuthenticatedUser();
            });

        return services;
    }
}
