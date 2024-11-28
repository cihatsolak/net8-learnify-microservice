namespace Learnify.Catalog.API.Features.Courses.Delete;

public sealed record DeleteCourseCommand(Guid Id) : IRequestResult;
