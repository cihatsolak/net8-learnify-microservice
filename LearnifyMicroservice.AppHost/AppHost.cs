using LearnifyMicroservice.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var catalogApi = builder.AddCatalogService();
var basketApi = builder.AddBasketService();
var discountApi = builder.AddDiscountService();

var fileApi = builder.AddProject<Projects.Learnify_File_API>("learnify-file-api");
var paymentApi = builder.AddProject<Projects.Learnify_Payment_API>("learnify-payment-api");

builder.AddProject<Projects.Learnify_Gateway>("learnify-gateway");

var orderApi = builder.AddOrderService();

var web = builder.AddProject<Projects.Learnify_Web>("learnify-web");

web.WithReference(basketApi)
    .WithReference(catalogApi)
    .WithReference(discountApi)
    .WithReference(orderApi)
    .WithReference(fileApi)
    .WithReference(paymentApi);

builder.Build().Run();
