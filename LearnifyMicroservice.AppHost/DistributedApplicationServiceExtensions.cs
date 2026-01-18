namespace LearnifyMicroservice.AppHost;

public static class DistributedApplicationServiceExtensions
{
    public static IResourceBuilder<ProjectResource> AddCatalogService(
        this IDistributedApplicationBuilder builder, 
        IResourceBuilder<RabbitMQServerResource> rabbitMq)
    {
        var mongoUserName = builder.AddParameter("MONGO-USERNAME");
        var mongoPassword = builder.AddParameter("MONGO-PASSWORD");

        var mongoCatalogDb = builder
            .AddMongoDB("mongo-db-catalog", 27030, mongoUserName, mongoPassword)
            .WithImage("mongo:8.0-rc")
            .WithDataVolume("mongo.db.catalog.volume")
            .AddDatabase("catalog-db");

        var catalogApi = builder.AddProject<Projects.Learnify_Catalog_API>("learnify-catalog-api");

        catalogApi.WithReference(mongoCatalogDb).WaitFor(mongoCatalogDb);
        catalogApi.WithReference(rabbitMq).WaitFor(rabbitMq);

        return catalogApi;
    }

    public static IResourceBuilder<ProjectResource> AddBasketService(
        this IDistributedApplicationBuilder builder, 
        IResourceBuilder<RabbitMQServerResource> rabbitMq)
    {
        var redisPassword = builder.AddParameter("REDIS-PASSWORD");

        var redisBasketDb = builder
            .AddRedis("redis-db-basket")
            .WithImage("redis:7.0-alpine")
            .WithDataVolume("redis.db.basket.volume")
            .WithPassword(redisPassword);

        var basketApi = builder.AddProject<Projects.Learnify_Basket_API>("learnify-basket-api");
        basketApi.WithReference(redisBasketDb).WaitFor(redisBasketDb);
        basketApi.WithReference(rabbitMq).WaitFor(rabbitMq);

        return basketApi;
    }

    public static IResourceBuilder<ProjectResource> AddDiscountService(
        this IDistributedApplicationBuilder builder, 
        IResourceBuilder<RabbitMQServerResource> rabbitMq)
    {
        var mongoUserName = builder.AddParameter("MONGO-USERNAME");
        var mongoPassword = builder.AddParameter("MONGO-PASSWORD");

        var mongoDiscountDb = builder
            .AddMongoDB("mongo-db-discount", 27034, mongoUserName, mongoPassword)
            .WithImage("mongo:8.0-rc")
            .WithDataVolume("mongo.db.discount.volume")
            .AddDatabase("discount-db");

        var discoutntApi = builder.AddProject<Projects.Learnify_Discount_API>("learnify-discount-api");

        discoutntApi.WithReference(mongoDiscountDb).WaitFor(mongoDiscountDb);
        discoutntApi.WithReference(rabbitMq).WaitFor(rabbitMq);

        return discoutntApi;
    }

    public static IResourceBuilder<ProjectResource> AddOrderService(
        this IDistributedApplicationBuilder builder, 
        IResourceBuilder<RabbitMQServerResource> rabbitMq)
    {
        var sqlServerPassword = builder.AddParameter("SQLSERVER-SA-PASSWORD");

        var sqlServerOrderDb = builder
            .AddSqlServer("sqlserver-db-order")
            .WithPassword(sqlServerPassword)
            .WithDataVolume("sqlserver.db.order.volume")
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .AddDatabase("order-db-aspire");

        var orderApi = builder.AddProject<Projects.Learnify_Order_API>("learnify-order-api");
        
        orderApi.WithReference(sqlServerOrderDb).WaitFor(sqlServerOrderDb);
        orderApi.WithReference(rabbitMq).WaitFor(rabbitMq);

        return orderApi;
    }
}

