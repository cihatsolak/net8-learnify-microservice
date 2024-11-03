namespace Learnify.Catalog.API.Features.Categories;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryResponse>();
    }
}