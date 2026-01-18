using LearnifyMicroservice.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddCatalogService();
builder.AddBasketService();
builder.AddDiscountService();


builder.AddProject<Projects.Learnify_File_API>("learnify-file-api");
builder.AddProject<Projects.Learnify_Payment_API>("learnify-payment-api");
builder.AddProject<Projects.Learnify_Gateway>("learnify-gateway");





builder.AddProject<Projects.Learnify_Order_API>("learnify-order-api");



builder.AddProject<Projects.Learnify_Web>("learnify-web");

builder.Build().Run();
