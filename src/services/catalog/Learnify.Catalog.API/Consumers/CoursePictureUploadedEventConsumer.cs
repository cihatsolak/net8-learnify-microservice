namespace Learnify.Catalog.API.Consumers;

public sealed class CoursePictureUploadedEventConsumer(
    IServiceProvider serviceProvider,
    ILogger<CoursePictureUploadedEventConsumer> logger)
        : IConsumer<CoursePictureUploadedEvent>
{
    public async Task Consume(ConsumeContext<CoursePictureUploadedEvent> context)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var course = await dbContext.Courses.FindAsync(context.Message.CourseId);
            if (course is null)
            {
                logger.LogWarning("Course with Id {CourseId} not found.", context.Message.CourseId);
                return; // veya özel exception fırlat
            }

            course.ImageUrl = context.Message.ImageUrl;
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Course {CourseId} image updated successfully.", context.Message.CourseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating course image for CourseId {CourseId}", context.Message.CourseId);
            throw; // hata bubble-up
        }
    }
}
