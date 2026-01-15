var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptionsExt<MongoDbOption>();
builder.Services.AddDatabaseServiceExt();
builder.Services.AddCommonServiceExt();
builder.Services.AddMassTransitExt(builder.Configuration);

builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

builder.Services.AddApiVersioningExt();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler(x => { });

app.AddCategoryGroupEndpointExt(app.GetVersionSetExt());
app.AddCourseGroupEndpointExt(app.GetVersionSetExt());

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

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();

