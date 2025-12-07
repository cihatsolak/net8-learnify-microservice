var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

builder.Services.AddSingleton<IBasketService, BasketService>();

builder.Services.AddCommonMassTransitExt(builder.Configuration);
builder.Services.AddApiVersioningExt();
builder.Services.AddCommonServiceExt();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

app.UseExceptionHandler(x => { });

app.AddBasketGroupEndpointExt(app.GetVersionSetExt());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();