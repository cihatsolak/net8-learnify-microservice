namespace Learnify.Bus.Commands;

public sealed record UploadCoursePictureCommand(Guid CourseId, byte[] Picture, string FileName);
