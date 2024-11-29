namespace Learnify.Catalog.API.Features.Categories.Create;

public static class CreateCategoryEndpoint
{
    public static RouteGroupBuilder CreateCategoryGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/", async (CreateCategoryCommand command, IMediator mediator)
            => await mediator.Send(command).ToGenericResultAsync())
            .WithName("CreateCategory")
            .MapToApiVersion(1,0)
            .AddEndpointFilter<ValidationFilter<CreateCategoryCommand>>();

        return routeGroupBuilder;
    }
}

public class CreateCategoryCommandHandler(AppDbContext context)
    : IRequestHandler<CreateCategoryCommand, ServiceResult<CreateCategoryResponse>>
{
    public async Task<ServiceResult<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        bool existsCategory = await context.Categories.AnyAsync(category => category.Name == request.Name, cancellationToken);
        if (existsCategory)
        {
            return ServiceResult<CreateCategoryResponse>.Error("Category name already exists",
                                                              $"The category name '{request.Name}' already exists",
                                                              StatusCodes.Status400BadRequest);
        }

        var category = new Category()
        {
            Name = request.Name,
            Id = NewId.NextSequentialGuid()
        };

        await context.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<CreateCategoryResponse>
            .SuccessAsCreated(new CreateCategoryResponse(category.Id), "<empty>");
    }
}