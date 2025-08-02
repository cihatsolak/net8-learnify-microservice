var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

builder.Services.AddOptionsExt<MongoDbOption>();
builder.Services.AddDatabaseServiceExt();
builder.Services.AddCommonServiceExt();
builder.Services.AddApiVersioningExt();

var app = builder.Build();

app.AddDiscountGroupEndpointExt(app.GetVersionSetExt());

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();