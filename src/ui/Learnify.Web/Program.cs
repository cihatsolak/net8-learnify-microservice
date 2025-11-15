using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddOptionsExt();

builder.Services.AddHttpClient<SignUpService>();
builder.Services.AddHttpClient<SignInService>();

builder.Services.AddScoped<TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<AuthenticatedHttpClientHandler>();
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();

builder.Services.AddAuthentication(configureOption =>
{
    configureOption.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    configureOption.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Auth/SignIn";
    options.LogoutPath = "/Auth/SignOut";
    options.Cookie.Name = "LearnifyWebCookie";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.Cookie.HttpOnly = true; //Çerezin yalnýzca sunucu tarafýnda eriþilebilir olmasýný saðlar (JavaScript eriþemez).
    options.SlidingExpiration = true; //Kullanýcý etkin olduðu sürece oturum süresini uzatýr.
});

builder.Services.AddAuthorization();

builder.Services.AddRefitClient<ICatalogRefitService>().ConfigureHttpClient(configure =>
{
    var microserviceOption = builder.Configuration.GetSection(nameof(MicroserviceOption)).Get<MicroserviceOption>();
    configure.BaseAddress = new Uri(microserviceOption!.Catalog.BaseAddress);
})
.AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
