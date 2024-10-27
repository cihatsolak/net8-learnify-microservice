var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptionsExt<MongoDbOption>();

builder.Services.AddDatabaseServiceExt();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddCategoryGroupEndpointExt();

await app.RunAsync();

