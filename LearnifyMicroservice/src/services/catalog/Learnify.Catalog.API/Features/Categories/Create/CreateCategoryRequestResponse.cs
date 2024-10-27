namespace Learnify.Catalog.API.Features.Categories.Create;

public sealed record CreateCategoryCommand(string Name) : IRequest<ServiceResult<CreateCategoryResponse>>;

public sealed record CreateCategoryResponse(Guid Id);
