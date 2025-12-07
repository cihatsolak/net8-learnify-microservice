var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);
builder.Services.AddCommonServiceExt();
builder.Services.AddApiVersioningExt();
builder.Services.AddCommonMassTransitExt(builder.Configuration);

builder.Services.AddRefitConfiguration(builder.Configuration);

builder.Services.AddHostedService<CheckPaymentStatusOrderBackgroundService>();

var app = builder.Build();

app.UseExceptionHandler(x => { });

app.AddOrderGroupEndpointExt(app.GetVersionSetExt());

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();
