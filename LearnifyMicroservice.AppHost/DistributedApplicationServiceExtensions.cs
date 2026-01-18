namespace LearnifyMicroservice.AppHost;

public static class DistributedApplicationServiceExtensions
{
    public static void AddCatalogService(
        this IDistributedApplicationBuilder builder)
    {
        var mongoUserName = builder.AddParameter("MONGO-USERNAME");
        var mongoPassword = builder.AddParameter("MONGO-PASSWORD");

        var mongoCatalogDb = builder
            .AddMongoDB("mongo-db-catalog", 27030, mongoUserName, mongoPassword)
            .WithImage("mongo:8.0-rc")
            .WithDataVolume("mongo.db.catalog.volume")
            .AddDatabase("catalog-db");

        var catalogApi = builder
            .AddProject<Projects.Learnify_Catalog_API>("learnify-catalog-api")
            .WithReference(mongoCatalogDb)
            .WaitFor(mongoCatalogDb);
    }

    public static void AddBasketService(
        this IDistributedApplicationBuilder builder)
    {
        var redisPassword = builder.AddParameter("REDIS-PASSWORD");

        var redisBasketDb = builder
            .AddRedis("redis-db-basket")
            .WithImage("redis:7.0-alpine")
            .WithDataVolume("redis.db.basket.volume")
            .WithPassword(redisPassword);

        builder
            .AddProject<Projects.Learnify_Basket_API>("learnify-basket-api")
            .WithReference(redisBasketDb)
            .WaitFor(redisBasketDb);
    }

    public static void AddDiscountService(
        this IDistributedApplicationBuilder builder)
    {
        var mongoUserName = builder.AddParameter("MONGO-USERNAME");
        var mongoPassword = builder.AddParameter("MONGO-PASSWORD");

        var mongoDiscountDb = builder
            .AddMongoDB("mongo-db-discount", 27034, mongoUserName, mongoPassword)
            .WithImage("mongo:8.0-rc")
            .WithDataVolume("mongo.db.discount.volume")
            .AddDatabase("discount-db");

        builder
            .AddProject<Projects.Learnify_Discount_API>("learnify-discount-api")
            .WithReference(mongoDiscountDb)
            .WaitFor(mongoDiscountDb);
    }
}

