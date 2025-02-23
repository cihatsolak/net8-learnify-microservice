namespace Learnify.File.API.Features.Upload;

public sealed class UploadFileCommandHandler(IFileProvider fileProvider) : IRequestHandler<UploadFileCommand, ServiceResult<UploadFileCommandResponse>>
{
    public async Task<ServiceResult<UploadFileCommandResponse>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        if (request.File.Length == 0)
        {
            return ServiceResult<UploadFileCommandResponse>.Error("Invalid file", "The provided file is empty or null", StatusCodes.Status400BadRequest);
        }

        string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}"; // .jpg
        string uploadPath = Path.Combine(fileProvider.GetFileInfo("files").PhysicalPath!, newFileName);

        await using var stream = new FileStream(uploadPath, FileMode.Create);
        await request.File.CopyToAsync(stream, cancellationToken);

        UploadFileCommandResponse response = new(newFileName, $"files/{newFileName}", request.File.FileName);

        return ServiceResult<UploadFileCommandResponse>.SuccessAsCreated(response, response.FilePath);
    }
}

public static class UploadFileCommandEndpoint
{
    public static RouteGroupBuilder UploadFileGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (IFormFile file, IMediator mediator) =>
                    await mediator.Send(new UploadFileCommand(file)).ToGenericResultAsync())
            .WithName("upload")
            .MapToApiVersion(1, 0)
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError).DisableAntiforgery();

        return group;
    }
}