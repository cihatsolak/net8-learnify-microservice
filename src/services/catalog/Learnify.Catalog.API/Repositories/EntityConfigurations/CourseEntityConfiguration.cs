namespace Learnify.Catalog.API.Repositories.EntityConfigurations;

public sealed class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToCollection("courses");
        builder.HasKey(course => course.Id);
        builder.Property(course => course.Id).ValueGeneratedNever();
        builder.Property(course => course.Name).HasElementName("name").HasMaxLength(100);
        builder.Property(course => course.Description).HasElementName("description").HasMaxLength(800);
        builder.Property(course => course.Created).HasElementName("created");
        builder.Property(course => course.UserId).HasElementName("user_id");
        builder.Property(course => course.CategoryId).HasElementName("category_id");
        builder.Property(course => course.Picture).HasElementName("picture");

        builder.OwnsOne(course => course.Feature, feature =>
        {
            feature.Property(feature => feature.Duration).HasElementName("duration");
            feature.Property(feature => feature.Rating).HasElementName("rating");
            feature.Property(feature => feature.EducatorFullName).HasElementName("educator_full_name").HasMaxLength(100);
        });

        builder.Ignore(course => course.Category);
    }
}