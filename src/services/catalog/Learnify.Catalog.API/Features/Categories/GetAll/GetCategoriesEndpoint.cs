namespace Learnify.Catalog.API.Features.Categories.GetAll;

public sealed record GetCategoriesQuery : IRequest<ServiceResult<List<CategoryResponse>>>;

public class GetCategoriesHandler(AppDbContext context, IMapper mapper) : IRequestHandler<GetCategoriesQuery, ServiceResult<List<CategoryResponse>>>
{
    public async Task<ServiceResult<List<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await context.Categories.ToListAsync(cancellationToken);
        var mappedCategories = mapper.Map<List<CategoryResponse>>(categories);

        return ServiceResult<List<CategoryResponse>>.SuccessAsOk(mappedCategories);
    }
}

public static class GetCategoriesEndpoint
{
    public static RouteGroupBuilder GetCategoriesGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("/", async (IMediator mediator) =>
        {
            return (await mediator.Send(new GetCategoriesQuery())).ToGenericResult();
        });

        return routeGroupBuilder;
    }
}