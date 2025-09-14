namespace Learnify.Bus.Events;

public sealed record CoursePictureUploadedEvent(Guid CourseId, string ImageUrl);
