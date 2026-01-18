namespace LearnifyMicroservice.AppHost;

public static class DistributedApplicationServiceExtensions
{
    public static void AddCatalogService(
        this IDistributedApplicationBuilder builder)
    {
        var mongoUserName = builder.AddParameter("MONGO-USERNAME");
        var mongoPassword = builder.AddParameter("MONGO-PASSWORD");

        var mongoCatalogDb = builder
            .AddMongoDB("mongo.db.catalog", 27030, mongoUserName, mongoPassword)
            .WithImage("mongo:8.0-rc")
            .WithDataVolume("mongo.db.catalog.volume")
            .AddDatabase("catalog-db");

        var catalogApi = builder
            .AddProject<Projects.Learnify_Catalog_API>("learnify-catalog-api")
            .WithReference(mongoCatalogDb)
            .WaitFor(mongoCatalogDb);
    }
}

