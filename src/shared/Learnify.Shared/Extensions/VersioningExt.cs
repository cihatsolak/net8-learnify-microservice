namespace Learnify.Shared.Extensions;

public static class VersioningExt
{
    public static IServiceCollection AddApiVersioningExt(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0); // Default versiyon
            options.AssumeDefaultVersionWhenUnspecified = true; //Belirtilmediğinde default versiyon kullanılsın
            options.ReportApiVersions = true; //Response header'da hangi versiyonların desteklendiğini gösterir
            options.ApiVersionReader = new UrlSegmentApiVersionReader(); //Versiyonu nereden alacağız?

            #region Birden fazla yerden versiyon bilgisini almak için
            //options.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader(),
            //    new QueryStringApiVersionReader(), new UrlSegmentApiVersionReader());
            #endregion

        })
        .AddApiExplorer(options => //Swagger için geçerli olan konfigürasyon kodu
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    //Minimal api'ler için bir versiyon seti oluşturmak
    public static ApiVersionSet GetVersionSetExt(this WebApplication app)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0)) //default olarak sahip olunan versiyonlar
            .HasApiVersion(new ApiVersion(1, 2)) //default olarak sahip olunan versiyonlar
            .HasApiVersion(new ApiVersion(2, 0)) //default olarak sahip olunan versiyonlar
            .ReportApiVersions()
            .Build();

        return apiVersionSet;
    }
}