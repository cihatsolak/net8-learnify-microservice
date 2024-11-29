namespace Learnify.Catalog.API.Features.Courses;

public static class CourseEndpointExt
{
    public static void AddCourseGroupEndpointExt(this WebApplication app)
    {
        app.MapGroup("api/courses")
            .WithTags("Courses")
            .CreateCourseGroupItemEndpoint()
            .GetCoursesGroupItemEndpoint()
            .GetCourseByIdGroupItemEndpoint()
            .UpdateCourseGroupItemEndpoint()
            .DeleteCourseGroupItemEndpoint()
            .GetCoursesByUserIdGroupItemEndpoint();
    }
}
