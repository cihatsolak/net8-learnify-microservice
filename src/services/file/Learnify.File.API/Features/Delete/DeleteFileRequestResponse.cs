namespace Learnify.File.API.Features.Delete;

public sealed record DeleteFileCommand(string FileName) : IRequestResult;
