namespace Learnify.Catalog.API.Features.Categories.GetAll;

public static class GetCategoriesEndpoint
{
    public static RouteGroupBuilder GetCategoriesGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("/", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetCategoriesQuery()).ToGenericResultAsync();
        })
        .WithName("GetCategories")
        .MapToApiVersion(1, 0);

        return routeGroupBuilder;
    }
}

public class GetCategoriesHandler(AppDbContext context, IMapper mapper) 
    : IRequestHandler<GetCategoriesQuery, ServiceResult<List<CategoryResponse>>>
{
    public async Task<ServiceResult<List<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await context.Categories
            .AsNoTracking()
            .ProjectTo<CategoryResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return ServiceResult<List<CategoryResponse>>.SuccessAsOk(categories);
    }
}
