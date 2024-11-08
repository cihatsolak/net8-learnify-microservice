namespace Learnify.Catalog.API.Features.Categories;

public static class CategoryEndpointExt
{
    public static void AddCategoryGroupEndpointExt(this WebApplication app)
    {
        app.MapGroup("api/categories")
            .WithTags("Categories")
            .CreateCategoryGroupItemEndpoint()
            .GetCategoriesGroupItemEndpoint()
            .GetCategoryByIdGroupItemEndpoint();
    }
}
