namespace Learnify.File.API.Features;

public static class FileEndpointExt
{
    public static void AddFileGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/files")
            .WithTags("files")
            .WithApiVersionSet(apiVersionSet)
            .UploadFileGroupItemEndpoint()
            .DeleteFileGroupItemEndpoint();
    }
}