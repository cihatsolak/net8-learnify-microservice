namespace Learnify.Catalog.API.Features.Courses.Create;

public sealed record CreateCourseCommand(string Name, 
                                         string Description, 
                                         decimal Price, 
                                         string ImageUrl,
                                         Guid CategoryId)
                                         : IRequestResult<Guid>;