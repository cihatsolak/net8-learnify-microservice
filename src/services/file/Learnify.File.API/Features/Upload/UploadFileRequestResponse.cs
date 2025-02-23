namespace Learnify.File.API.Features.Upload;

public sealed record UploadFileCommand(IFormFile File) : IRequestResult<UploadFileCommandResponse>;

public sealed record UploadFileCommandResponse(string FileName, string FilePath, string OriginalFileName);