namespace Learnify.Catalog.API.Features.Courses;

public sealed class Course : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid UserId { get; set; }
    public string Picture { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public Feature Feature { get; set; }
}