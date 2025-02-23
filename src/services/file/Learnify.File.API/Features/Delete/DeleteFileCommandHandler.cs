namespace Learnify.File.API.Features.Delete;

public sealed class DeleteFileCommandHandler(IFileProvider fileProvider) : IRequestHandler<DeleteFileCommand, ServiceResult>
{
    public Task<ServiceResult> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var fileInfo = fileProvider.GetFileInfo(Path.Combine("files", request.FileName));

        if (!fileInfo.Exists)
        {
            return Task.FromResult(ServiceResult.ErrorAsNotFound());
        }

        System.IO.File.Delete(fileInfo.PhysicalPath!);

        return Task.FromResult(ServiceResult.SuccessAsNoContent());
    }
}

public static class DeleteFileCommandEndpoint
{
    public static RouteGroupBuilder DeleteFileGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("",
                async ([FromBody] DeleteFileCommand deleteFileCommand, IMediator mediator) =>
                await mediator.Send(deleteFileCommand).ToGenericResultAsync())
            .WithName("delete")
            .MapToApiVersion(1, 0)
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return group;
    }
}