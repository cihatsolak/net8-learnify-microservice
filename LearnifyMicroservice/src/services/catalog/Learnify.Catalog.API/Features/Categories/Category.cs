namespace Learnify.Catalog.API.Features.Categories;

public sealed class Category : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public List<Course> Courses { get; set; }
}
