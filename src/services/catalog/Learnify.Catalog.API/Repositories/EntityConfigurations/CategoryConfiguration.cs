namespace Learnify.Catalog.API.Repositories.EntityConfigurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToCollection("categories");
        builder.HasKey(category => category.Id);
        builder.Property(category => category.Id).ValueGeneratedNever();

        builder.Property(category => category.Name).HasElementName("name");
        builder.Property(category => category.Description).HasElementName("description");
        builder.Property(category => category.ImageUrl).HasElementName("image_url");

        builder.Ignore(course => course.Courses);
    }
}
