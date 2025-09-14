namespace Learnify.File.API.Consumers;

public sealed class UploadCoursePictureCommandConsumer(IServiceProvider serviceProvider)
        : IConsumer<UploadCoursePictureCommand>
{
    public async Task Consume(ConsumeContext<UploadCoursePictureCommand> context)
    {
        using var scope = serviceProvider.CreateScope();
        IFileProvider fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();
        IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(context.Message.FileName)}"; // .jpg
        string uploadPath = Path.Combine(fileProvider.GetFileInfo("files").PhysicalPath, newFileName);

        await System.IO.File.WriteAllBytesAsync(uploadPath, context.Message.Picture);

        CoursePictureUploadedEvent coursePictureUploadedEvent = new(context.Message.CourseId, $"files/{newFileName}");
        await publishEndpoint.Publish(coursePictureUploadedEvent);
    }
}