namespace Learnify.Catalog.API.Repositories;

public static class RepositoryExt
{
    public static IServiceCollection AddDatabaseServiceExt(this IServiceCollection services)
    {
        services.AddScoped<AppDbContext>(static provider =>
        {
            var mongoDbOption = provider.GetRequiredService<IOptions<MongoDbOption>>();
            MongoClient mongoClient = new(mongoDbOption.Value.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(mongoDbOption.Value.DatabaseName);

            return AppDbContext.Create(mongoDatabase);
        });

        return services;
    }
}
