using LearnifyMicroservice.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var rabbitmqUserName = builder.AddParameter("RABBITMQ-USERNAME");
var rabbitmqPassword = builder.AddParameter("RABBITMQ-PASSWORD");

var rabbitMq = builder.AddRabbitMQ("rabbitMQ", rabbitmqUserName, rabbitmqPassword, 5672)
                      .WithManagementPlugin(15672);

var catalogApi = builder.AddCatalogService(rabbitMq);
var basketApi = builder.AddBasketService(rabbitMq);
var discountApi = builder.AddDiscountService(rabbitMq);

var fileApi = builder.AddProject<Projects.Learnify_File_API>("learnify-file-api")
                        .WithReference(rabbitMq)
                        .WaitFor(rabbitMq);

var paymentApi = builder.AddProject<Projects.Learnify_Payment_API>("learnify-payment-api")
                            .WithReference(rabbitMq)
                            .WaitFor(rabbitMq);

builder.AddProject<Projects.Learnify_Gateway>("learnify-gateway");

var orderApi = builder.AddOrderService(rabbitMq);

var web = builder.AddProject<Projects.Learnify_Web>("learnify-web");

web.WithReference(basketApi)
    .WithReference(catalogApi)
    .WithReference(discountApi)
    .WithReference(orderApi)
    .WithReference(fileApi)
    .WithReference(paymentApi);

builder.Build().Run();
