namespace Learnify.Catalog.API.Features.Categories.GetById;

public sealed record GetCategoryByIdRequest(Guid Id) : IRequestResult<CategoryResponse>;

public sealed class GetCategoryByIdHandler(AppDbContext context, IMapper mapper) 
    : IRequestHandler<GetCategoryByIdRequest, ServiceResult<CategoryResponse>>
{
    public async Task<ServiceResult<CategoryResponse>> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);
        if (category is null)
        {
            return ServiceResult<CategoryResponse>.
                Error("Category not found",$"The category with Id ({request.Id}) was not found.", StatusCodes.Status404NotFound);
        }

        return ServiceResult<CategoryResponse>.SuccessAsOk(mapper.Map<CategoryResponse>(category));
    }
}

public static class GetCategoryByIdEndpoint
{
    public static RouteGroupBuilder GetCategoryByIdGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            return await mediator.Send(new GetCategoryByIdRequest(id)).ToGenericResultAsync();
        });

        return routeGroupBuilder;
    }
}
