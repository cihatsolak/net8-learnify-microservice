var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptionsExt<MongoDbOption>();
builder.Services.AddDatabaseServiceExt();
builder.Services.AddCommonServiceExt();

builder.Services.AddApiVersioningExt();

var app = builder.Build();

_ = app.AddSeedDataExtAsync().ContinueWith(task =>
{
    if (task.IsFaulted)
    {
        Console.WriteLine(task.Exception?.Message);
    }
    else
    {
        Console.WriteLine("Seed data has been saved successfully.");
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddCategoryGroupEndpointExt(app.GetVersionSetExt());
app.AddCourseGroupEndpointExt(app.GetVersionSetExt());

await app.RunAsync();

