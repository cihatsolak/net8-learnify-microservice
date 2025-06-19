var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCommonServiceExt();

builder.Services.AddApiVersioningExt();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("payment-in-memory-db");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}


await app.RunAsync();