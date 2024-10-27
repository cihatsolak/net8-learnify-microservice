namespace Learnify.Catalog.API.Features.Categories;

public static class CategoryEndpointExt
{
    public static void AddCategoryGroupEndpointExt(this WebApplication app)
    {
        app.MapGroup("app/categories")
            .CreateCategoryGroupItemEndpoint()
            .RequireAuthorization();
    }
}
