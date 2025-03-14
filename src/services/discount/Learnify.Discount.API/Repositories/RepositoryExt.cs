﻿namespace Learnify.Discount.API.Repositories;

public static class RepositoryExt
{
    public static IServiceCollection AddDatabaseServiceExt(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient, MongoClient>(sp =>
        {
            var options = sp.GetRequiredService<MongoDbOption>();
            return new MongoClient(options.ConnectionString);
        });

        services.AddScoped<AppDbContext>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var options = sp.GetRequiredService<MongoDbOption>();

            return AppDbContext.Create(mongoClient.GetDatabase(options.DatabaseName));
        });


        return services;
    }
}
